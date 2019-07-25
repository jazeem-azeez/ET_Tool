using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using ET_Tool.Common.Logger;
using Newtonsoft.Json;

namespace ET_Tool.Business.DataSink.DataSinkKinds
{
    public class CsvDataSinkKind : IDisposable
    {
        private const string RowDelimiterSeparator= " \t ";
        private const string HeaderDelimiterSeparator= "|";

        //protected CsvWriter _csvWriter;
        protected readonly IEtLogger _logger;
        private readonly string _destTemplateConfigurationFile;
        protected readonly string _destFileName;
        protected StreamWriter _streamWriter;
        public Dictionary<string, string> OutInfo { get; private set; }

        public CsvDataSinkKind(string destFileName, IEtLogger etLogger, string destTemplateConfigurationFile)
        {
            this._destFileName = destFileName;
            this._logger = etLogger;
            this._destTemplateConfigurationFile = destTemplateConfigurationFile;
            _streamWriter = new StreamWriter(destFileName);
            //_csvWriter = new CsvWriter(_streamWriter);
        }
        public bool LoadTemplate()
        {
            this.OutInfo = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(this._destTemplateConfigurationFile));            
            return true;
        }
        public bool AddHeader(string[] header)
        {
            _streamWriter.WriteLine(string.Join('|', header)); 
            return true;
        }
        public bool AddRow(string[] row)
        {
            _streamWriter.WriteLine(string.Join('\t', row)); 
            return false;
        }
        public void Dispose()
        {
            this._streamWriter.Dispose(); 
        }

    }
}
