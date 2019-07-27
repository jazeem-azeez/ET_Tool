using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using ET_Tool.Common.Logger;
using ET_Tool.Common.Models;
using Newtonsoft.Json;

namespace ET_Tool.Business.DataSink.DataSinkKinds
{
    public class CsvDataSinkKind : IDisposable
    {
        protected readonly string _destFileName;

        //protected CsvWriter _csvWriter;
        protected readonly IEtLogger _logger;

        protected StreamWriter _streamWriter;
        protected string HeaderDelimiterSeparator;
        protected string RowDelimiterSeparator;
        private const string headersKey = "Headers";
        private readonly string _destTemplateConfigurationFile;

        public CsvDataSinkKind(string destFileName, IEtLogger etLogger, string destTemplateConfigurationFile)
        {
            this._destFileName = destFileName;
            this._logger = etLogger;
            this._destTemplateConfigurationFile = destTemplateConfigurationFile;
            this.Initialize();
        }

        public string[] Columns { get; private set; }
        public Dictionary<string, string> OutInfo { get; private set; }

        public void AddRecordsToSink(string[] row) => this._streamWriter.WriteLine(string.Join(',', row));

        public void AddRecordsToSink(List<DataCell> cells) => this.AddRecordsToSink(cells.Select(c => c.Value).ToArray());
        public void Dispose() => this._streamWriter.Dispose();

        public void Initialize()
        {
            this.LoadOutpuConfiguration();
            this._streamWriter = new StreamWriter(this._destFileName);
            this.AddHeader(this.Columns);
        }

        public void LoadOutpuConfiguration()
        {
            this.OutInfo = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(this._destTemplateConfigurationFile));
            this.Columns = this.OutInfo[headersKey].Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (this.OutInfo.ContainsKey(headersKey) == false)
            {
                this._logger.Log("Missing Output Header Information", EventLevel.Critical);
                throw new InvalidDataException("Missing Headers Info");
            }
            this.RowDelimiterSeparator = this.OutInfo[nameof(this.RowDelimiterSeparator)];
            this.HeaderDelimiterSeparator = this.OutInfo[nameof(this.HeaderDelimiterSeparator)];
        }

        private bool AddHeader(string[] header)
        {
            this._streamWriter.WriteLine(string.Join(',', header));
            return true;
        }
    }
}