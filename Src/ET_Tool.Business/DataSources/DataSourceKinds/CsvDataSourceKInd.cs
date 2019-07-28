using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Text;
using ET_Tool.Common;
using ET_Tool.Common.Logger;

using LumenWorks.Framework.IO.Csv;

namespace ET_Tool.Business.DataSourceKinds
{
    public class CsvDataSourceKInd : IDisposable
    {
        protected readonly IEtLogger _logger;
        protected readonly IDataCleaner _dataCleaner;
        protected readonly string _sourceFileName;
        protected StreamReader _streamReader;

        public CsvDataSourceKInd(string sourceFileName, IDataCleaner dataCleaner, IEtLogger logger)
        {
            this._sourceFileName = sourceFileName;
            this._logger = logger;
            this._dataCleaner = dataCleaner;
            this.Columns = new List<Column>();
        }

        public List<Column> Columns { get; private set; }

        public void Dispose()
        { 
            this._streamReader.Dispose();
        }

        public bool Init()
        {
            this._logger.Log($"Preparing to Load Headers from {this._sourceFileName}", EventLevel.LogAlways);
            FileInfo fileInfo = new FileInfo(this._sourceFileName);
            this._logger.ShowTable($"File Properties {fileInfo.FullName}", new string[] { "Name", "Value" }, new List<string[]>
            {
                new string [] {nameof(fileInfo.Attributes),fileInfo.Attributes.ToString()},
                new string [] {nameof(fileInfo.Length),(fileInfo.Length/1024)+" KB"},
                new string [] {nameof(fileInfo.CreationTime),fileInfo.CreationTime.ToString()},
                new string [] {nameof(fileInfo.LastAccessTime),fileInfo.LastAccessTime.ToString()},
                new string [] {nameof(fileInfo.Extension),fileInfo.Extension.ToString()},
            }
            );
            //TODO : update to use IdiskIohandler
            this._streamReader = new StreamReader(this._sourceFileName);

            string headerLine = _streamReader.ReadLine();
            string[] headerRow = CsvParseHelper.GetAllFields(headerLine);
            this.Columns.AddRange(headerRow.Select(item=>new Column() {Name=item}).ToArray());
            this._dataCleaner.CleanHeader(this.Columns);
            this._logger.Log($"Loaded file Headers from {this._sourceFileName}", EventLevel.LogAlways);

            return true;
        }

        private void Csv_ParseError(object sender, ParseErrorEventArgs e)
        {
            this._logger.Log($"--Parse Error OCCURRED, on {e.Error.CurrentRecordIndex}", EventLevel.Error);

        
        }
    }
}