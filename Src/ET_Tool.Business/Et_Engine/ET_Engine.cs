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

        public bool Init()
        {
            if (this._runtimeSettings.AutoBuild == true)
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
            }

            return true;
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

        public bool RunDataAnalysis()
        {
            int csvLines = 1, textLines = 0;
            using (IDataSource dataSource = this._dataSourceFactory.GetDataSource(this._runtimeSettings.DataSourceFileName))
            {
                foreach (DataCellCollection row in dataSource.GetDataRowEntries())
                {
                    csvLines += 1;
                }
            }
            using (StreamReader stream = new StreamReader(this._diskIOHandler.FileReadTextStream(this._runtimeSettings.DataSourceFileName)))
            {
                while (stream.EndOfStream == false)
                {
                    stream.ReadLine();
                    textLines += 1;
                }
            }
            if (textLines == csvLines)
            {
                this._logger.LogInformation("Text to record size mateched");
                return true;
            }
            else
            {
                this._logger.Log($"Found mismatch in number of textLines {textLines} & Csv Lines {csvLines} - Please clean & make sure all the data is properly parsable", EventLevel.Error);
                string dataSourceFileName = this._runtimeSettings.DataSourceFileName;
                return this.PerformAutoClean(dataSourceFileName, Path.GetFileNameWithoutExtension(dataSourceFileName) + "-csvdef.json");
            }
        }

        public bool PerformAutoClean(string dataSourceFileName, string csvTypeDef)
        {
            string tempId = Guid.NewGuid().ToString().Replace("-", "");
            this._logger.LogInformation($"Attempting AutoClean : operationId{tempId}");

            if (this._diskIOHandler.FileExists(csvTypeDef))
            {
                Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(this._diskIOHandler.FileReadAllText(csvTypeDef));

                using (StreamWriter streamWriter = new StreamWriter(this._diskIOHandler.FileWriteTextStream(tempId)))
                {
                    using (StreamReader streamReader = new StreamReader(this._diskIOHandler.FileReadTextStream(this._runtimeSettings.DataSourceFileName)))
                    {
                        string []headerRow = CsvParseHelper.GetAllFields(streamReader.ReadLine());

                        while (streamReader.EndOfStream == false)
                        {
                            string[] data = CsvParseHelper.GetAllFields(streamReader.ReadLine());
                            if (data != null && data.Length > 0)
                            {
                                for (int i = 0; i < data.Length; i++)
                                {
                                    switch (dictionary[headerRow[i]])
                                    {
                                        case "num": break;
                                        case "literal":
                                            data[i] = data[i].StartsWith('"') ? data[i] : '"' + data[i];
                                            data[i] = data[i].EndsWith('"') ? data[i] : data[i] + '"';
                                            break;
                                        default: break;
                                    }
                                }
                            }
                            streamWriter.WriteLine(string.Join(",", data));
                        }
                    }
                }
                return true;
            }
            return false;
        }
    }
}