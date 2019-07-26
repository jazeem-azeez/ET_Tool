using System;
using System.Collections.Generic;
using System.Linq;

using ET_Tool.Common.Logger;

namespace ET_Tool.Business.DataSourceKinds
{
    public class CsvDataSource : CsvDataSourceKInd, IDataSource
    {
        public CsvDataSource(string sourceFileName, IDataCleaner dataCleaner, IEtLogger logger) : base(sourceFileName, dataCleaner, logger) => this.IsDataClean();

        public IEnumerable<List<KeyValuePair<string, string>>> GetDataRowEntries()
        {
            int fieldCount = this._csvReader.FieldCount;
            string[] row = new string[fieldCount];

            while (this._csvReader.ReadNextRecord())
            {
                for (int i = 0; i < fieldCount; i++)
                {
                    row[i] = this._csvReader[i];
                }
                yield return this.BuildRow(row);
            }
        }

        public string[] GetHeaders() => this.Columns.Select(c => c.Name).ToArray();

        public bool IsDataClean() => this.Init() && this.Columns != null && this.Columns.Count > 0;

        private List<KeyValuePair<string, string>> BuildRow(string[] items)
        {
            if (items.Length != this.Columns.Count)
            {
                throw new DataMisalignedException("Columns & Values are not same");
            }
            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();
            for (int i = 0; i < this.Columns.Count; i++)
            {
                result.Add(new KeyValuePair<string, string>(this.Columns[i].Name, items[i]));
            }
            return _dataCleaner.CleanRow(result);
        }
    }
}