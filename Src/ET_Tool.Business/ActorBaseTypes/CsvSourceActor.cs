using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IO;

using CsvHelper;

using ET_Tool.Common.Logger;

namespace ET_Tool.Business.IngestActor
{
    public class CsvSourceActorBase : IET_Actor, IDisposable
    {
        private readonly CsvReader _csvReader;
        private readonly IEtLogger _logger;
        private readonly string _sourceFileName;
        private readonly StreamReader _streamReader;

        public CsvSourceActorBase(string sourceFileName, IEtLogger logger)
        {
            this._sourceFileName = sourceFileName;
            this._logger = logger;
            this.Columns = new List<string>();
        }

        public List<string> Columns { get; private set; }

        public void Dispose()
        {
            this._streamReader.Dispose();
            this._csvReader.Dispose();
        }

        public bool Init()
        {
            using (StreamReader reader = new StreamReader(this._sourceFileName))
            {
                using (CsvReader csv = new CsvReader(reader))
                {
                    csv.Read();
                    csv.ReadHeader();
                    foreach (string item in csv.Context.HeaderRecord)
                    {
                        this.Columns.Add(item);
                        csv.Configuration.BadDataFound = this.BadDataHandler;
                        csv.Configuration.MissingFieldFound = this.MissingFieldFoundHandler;
                    }
                }
            }
            return true;
        }

        public void Run()
        {
        }

        private void BadDataHandler(ReadingContext obj) => this._logger.Log($"Bad Data Found in Row: {obj.Row} \n Raw Data \n {obj.RawRow} ", EventLevel.Error, new InvalidDataException());

        private void MissingFieldFoundHandler(string[] arg1, int arg2, ReadingContext obj) => this._logger.Log($"Missing Field Found in Row: {obj.Row} \n Raw Data \n {obj.RawRow} \n args {string.Join(" ", arg1)} index {arg2}",
            EventLevel.Error,
            new System.MissingFieldException());
    }
}