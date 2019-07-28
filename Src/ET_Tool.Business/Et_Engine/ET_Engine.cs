using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;

using ET_Tool.Business.Mappers;
using ET_Tool.Common;
using ET_Tool.Common.IO;
using ET_Tool.Common.Logger;
using ET_Tool.Common.Models;

using LumenWorks.Framework.IO.Csv;

using Newtonsoft.Json;

namespace ET_Tool.Business
{
    public class ET_Engine : IET_Engine
    {
        private readonly IDataResolver _dataMapHandler;
        private readonly IDataSinkFactory _dataSinkFactory;
        private readonly IDataSourceFactory _dataSourceFactory;
        private readonly IDiskIOHandler _diskIOHandler;
        private readonly IEtLogger _logger;
        private readonly RuntimeArgs _runtimeSettings;
        private readonly SourceToSinkDataChainBuilder _toSinkDataChainBuilder;

        public ET_Engine(IDataSourceFactory dataSourceFactory,
                        IDataResolver dataResolver,
                        IDataSinkFactory dataSinkFactory,
                        IEtLogger logger,
                        IDiskIOHandler diskIOHandler,
                        RuntimeArgs runtimeSettings)
        {
            this._dataSourceFactory = dataSourceFactory;
            this._dataMapHandler = dataResolver;
            this._dataSinkFactory = dataSinkFactory;
            this._logger = logger;
            this._diskIOHandler = diskIOHandler;
            this._runtimeSettings = runtimeSettings;
            this._toSinkDataChainBuilder = new SourceToSinkDataChainBuilder(logger);
        }

        public void Dispose() => throw new NotImplementedException();

        public bool InitializePrepocessing()
        {
            try
            {
                string[] sources = this._diskIOHandler.DirectoryGetFiles(Path.GetDirectoryName(this._runtimeSettings.SourceDataFolder), this._runtimeSettings.LookUpFilePattern, SearchOption.AllDirectories);
                foreach (string item in sources)
                {
                    using (IDataSource dataSource = this._dataSourceFactory.GetDataSource(item))
                    {
                        DataLookUpCollection lookUpCollection = new DataLookUpCollection(dataSource, this._logger);
                        string key = Path.GetFileName(item);
                        this._toSinkDataChainBuilder.LookUps.Add(key, new HashSet<string>(dataSource.GetHeaders()));
                        this._dataMapHandler.AddNewDataLookUp(key, lookUpCollection);
                    }
                }
                foreach (IDataMapper item in this._dataMapHandler.GetAllMappers())
                {
                    this._toSinkDataChainBuilder.LookUps.Add(item.Name, item.GetAssociatedColumns());
                }
                using (IDataSource dataSource = this._dataSourceFactory.GetDataSource(this._runtimeSettings.DataSourceFileName))
                {
                    this._toSinkDataChainBuilder.AddSourceColumns(dataSource.GetHeaders());
                }
                using (IDataSink dataSink = this._dataSinkFactory.GetDataSink(this._runtimeSettings.DataSinkFileName, this._runtimeSettings.OutConfigFileName))
                {
                    this._toSinkDataChainBuilder.AddSinkColumns(dataSink.Columns);
                }
                Dictionary<string, List<string>> mappingRules = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(this._diskIOHandler.FileReadAllText(this._runtimeSettings.MappingRulesSourcePath));
                foreach (KeyValuePair<string, List<string>> item in mappingRules)
                {
                    this._dataMapHandler.AddNewMappingRule(item.Key, item.Value);
                }

                this._toSinkDataChainBuilder.BuildChain();
                return true;
            }
            catch (Exception ex)
            {
                this._logger.Log("Initialization Failed : see logs for mor information", EventLevel.Error, ex);
            }
            return false;
        }

        public bool PerformAutoClean(string dataSourceFileName, string csvTypeDef, int attempt)
        {
            string tempId = Guid.NewGuid().ToString().Replace("-", "") + ".csv";
            this._logger.LogInformation($"Attempting AutoClean : operationId{tempId}");

            if (this._diskIOHandler.FileExists(csvTypeDef))
            {
                // type based cleaning ; not implemented
                Dictionary<string, List<KeyValuePair<string, string>>> dictionary = JsonConvert.DeserializeObject<Dictionary<string, List<KeyValuePair<string, string>>>>(this._diskIOHandler.FileReadAllText(csvTypeDef));
            }

            int index = 0;
            using (StreamWriter streamWriter = new StreamWriter(this._diskIOHandler.FileWriteTextStream(tempId)))
            {
                using (StreamReader streamReader = new StreamReader(this._diskIOHandler.FileReadTextStream(dataSourceFileName)))
                {
                    string line = streamReader.ReadLine();
                    string[] headerRow = CsvParseHelper.GetAllFields(line);
                    streamWriter.WriteLine(line);

                    while (streamReader.EndOfStream == false)
                    {
                        line = streamReader.ReadLine();
                        string[] data = CsvParseHelper.GetAllFields(line);
                        // treat data misalliggnment
                        if (data.Length != headerRow.Length)
                        {
                            string[] lines = this.TreatMisAlignment(tempId, index, headerRow, line, data);
                            foreach (string alignedLine in lines)
                            {
                                streamWriter.WriteLine(alignedLine);
                            }
                        }
                        else
                        {
                            streamWriter.WriteLine(string.Join(",", data));
                        }

                        index++;
                    }
                }
            }
            this._diskIOHandler.FileCopy(dataSourceFileName, $"{dataSourceFileName}{tempId}.bak");
            this._diskIOHandler.FileCopy(tempId, dataSourceFileName, true);
            return this.RunDataAnalysis(attempt);
        }

        public void PerformTransformation()
        {
            int ingestRowsCount = 0;
            int egressRowsCount = 0;

            using (IDataSource dataSource = this._dataSourceFactory.GetDataSource(this._runtimeSettings.DataSourceFileName))
            {
                using (IDataSink dataSink = this._dataSinkFactory.GetDataSink(this._runtimeSettings.DataSinkFileName, this._runtimeSettings.OutConfigFileName))
                {
                    string[] rowValues = new string[dataSink.Columns.Length];
                    Dictionary<string, string> context = new Dictionary<string, string>();
                    foreach (DataCellCollection row in dataSource.GetDataRowEntries())
                    {
                        ingestRowsCount += 1;
                        DataCellCollection outRowCollection = new DataCellCollection();
                        for (int i = 0; i < dataSink.Columns.Length; i++)
                        {
                            Dictionary<string, string> steps = this._toSinkDataChainBuilder.GetSteps(dataSink.Columns[i]);

                            try
                            {
                                outRowCollection = this._dataMapHandler.Resolve(row, new Column() { Name = dataSink.Columns[i] }, outRowCollection, steps, context);
                            }
                            catch (Exception ex)
                            {
                                this._logger.Log($"resolving failed for {dataSink.Columns[i]} \n raw row data {row.Cells.Select(c => $"{c.Column.Name}: {c.Value}").ToArray()}", EventLevel.Error, ex);
                            }
                        }
                        if (outRowCollection.Cells.Count != dataSink.Columns.Length)
                        {
                            this._logger.Log($"resolving failed for raw row data {row.Cells.Select(c => $"{c.Column.Name}: {c.Value}").ToArray()}", EventLevel.Error);
                            continue;
                        }
                        dataSink.AddRecordsToSink(outRowCollection.Cells);
                        egressRowsCount += 1;
                    }
                }
            }
            this._logger.LogInformation($"Ingest = {ingestRowsCount} egress={egressRowsCount}");
        }

        public bool RunDataAnalysis(int attempt = 0)
        {
            if (attempt > 2)
            {
                this._logger.Log("Attempt Limit has reached , Aborting analiysys", EventLevel.Error);
                return false;
            }

            this._logger.LogInformation("Runing Analysis");

            this._logger.LogInformation("Scanning using text parser started");

            int csvLines = 0, textLines = 0, noError = 0; 
            using (StreamReader stream = new StreamReader(this._diskIOHandler.FileReadTextStream(this._runtimeSettings.DataSourceFileName)))
            {
                int headerCount = CsvParseHelper.GetAllFields(stream.ReadLine()).Length;

                while (stream.EndOfStream == false)
                {
                    string line = stream.ReadLine();
                    int cellcount = CsvParseHelper.GetAllFields(line).Length;
                    if (cellcount != headerCount)
                    {
                        this._logger.Log($"Error Data Alignment mismatch cellcount {cellcount } != headerCount {headerCount } att position {textLines} , line :{line}", EventLevel.Error);
                        noError++;
                    }

                    textLines += 1;
                }
            }
            this._logger.LogInformation($"Scanning using text parser completed with {noError} errors ");
            noError = 0;
            this._logger.LogInformation("Scanning using csv parser started");

            using (IDataSource dataSource = this._dataSourceFactory.GetDataSource(this._runtimeSettings.DataSourceFileName))
            {
                int headerCount = dataSource.GetHeaders().Length;
                foreach (DataCellCollection row in dataSource.GetDataRowEntries())
                {
                    if (row.Cells.Count != headerCount)
                    {
                        this._logger.Log($"Error Data Alignment mismatch via csv parser cellcount {row.Cells.Count} != headerCount {headerCount } att position {csvLines}", EventLevel.Error);
                        noError++;
                    }
                    csvLines += 1;
                }
            }
            this._logger.LogInformation($"Scanning using csv parser completed with {noError} errors");

            if (textLines == csvLines && noError == 0)
            {
                this._logger.LogInformation("Text to record size mateched & alignment test passed ");
                return true;
            }
            else
            {
                this._logger.Log($"Found mismatch in number of textLines {textLines} & Csv Lines {csvLines} - Please clean & make sure all the data is properly parsable", EventLevel.Error);
                string dataSourceFileName = this._runtimeSettings.DataSourceFileName;
                return this.PerformAutoClean(dataSourceFileName, Path.GetFileNameWithoutExtension(dataSourceFileName) + "-csvdef.json", attempt + 1);
            }
        }

        private string[] TreatMisAlignment(string tempId, int index, string[] headerRow, string line, string[] data)
        {
            List<string> fixedLines = new List<string>();
            string fixedLineItem = string.Empty;
            this._logger.LogInformation($"Attempting Auto Alignment fix for index {index} in operationId{tempId}");
            if (data.Length > headerRow.Length)
            {
                this._logger.LogInformation("input Line :" + line);

                for (int i = 0; i < data.Length; i += (headerRow.Length))
                {
                    string[] items = data.Skip(i).Take(headerRow.Length).ToArray();
                    if (items.Length != headerRow.Length)
                    {
                        fixedLineItem = GetAssist(index, line,items);
                    }
                    else
                    {
                        fixedLineItem = string.Join(",", items);
                    }
                    _logger.LogInformation(fixedLineItem); 
                    fixedLines.Add(fixedLineItem);
                }

                return fixedLines.ToArray();
            }
            else
            {
                fixedLineItem = GetAssist(index, line,data);
                fixedLines.Add(fixedLineItem);
            }

            return fixedLines.ToArray();
        }

        private string GetAssist(int index, string line, string[] items)
        {
            string input;
            this._logger.LogInformation($"Unable to Auto fix for \r\n line {index} : {line} \r\n data [{string.Join(",",items)}] ");
            Console.WriteLine("Please provide correct data: line ");
            input = Console.ReadLine();
            return input;
        }
    }
}