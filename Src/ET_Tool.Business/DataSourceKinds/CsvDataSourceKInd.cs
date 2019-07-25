using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Tracing;
using System.IO;

using CsvHelper;

using ET_Tool.Common.Logger;

namespace ET_Tool.Business.DataSourceKinds
{
    public class CsvDataSourceKInd : IDisposable
    {
        protected CsvReader _csvReader;
        protected CsvParser _csvParser;
        protected readonly IEtLogger _logger;
        protected readonly string _sourceFileName;
        protected StreamReader _streamReader;

        public CsvDataSourceKInd(string sourceFileName, IEtLogger logger)
        {
            this._sourceFileName = sourceFileName;
            this._logger = logger;
            this.Columns = new List<string>();
        }

        public List<string> Columns { get; private set; }

        public void Dispose()
        {
            this._csvReader.Dispose();
            this._csvParser.Dispose();
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
            this._streamReader = new StreamReader(this._sourceFileName);
            this._csvParser = new CsvParser(this._streamReader);
            this._csvReader = new CsvReader(this._csvParser);

            this._csvReader.Configuration.BadDataFound = this.BadDataHandler;
            this._csvReader.Configuration.MissingFieldFound = this.MissingFieldFoundHandler;

            this._csvReader.Read();
            this._csvReader.ReadHeader();

            foreach (string item in this._csvReader.Context.HeaderRecord)
            {
                this.Columns.Add(item);
            }


            this._logger.Log($"Loaded file Headers from {this._sourceFileName}", EventLevel.LogAlways);
           
            return true;
        }

        protected string[] GetRowData() => this._csvParser.Read();



        protected void BadDataHandler(ReadingContext obj) 
            => this._logger.Log($"Bad Data Found in Row: {obj.Row} \n Raw Data \n {obj.RawRow} ",
                EventLevel.Error, 
                new InvalidDataException());

        protected void MissingFieldFoundHandler(string[] arg1, int arg2, ReadingContext obj) 
            => this._logger.Log($"Missing Field Found in Row: {obj.Row} \n Raw Data \n {obj.RawRow} \n args {string.Join(" ", arg1)} index {arg2}",
            EventLevel.Error,
            new System.MissingFieldException());
    }
}