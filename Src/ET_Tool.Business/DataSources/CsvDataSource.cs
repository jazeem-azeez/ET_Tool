using System.Collections.Generic;
using ET_Tool.Common.Logger;

namespace ET_Tool.Business.DataSourceKinds
{
    public class CsvDataSource : CsvDataSourceKInd, IDataSource
    {
        public CsvDataSource(string sourceFileName, IEtLogger logger) : base(sourceFileName, logger) => this.IsDataClean();

        public string[] GetHeaders() => this.Columns.ToArray();
        public bool IsDataClean() => this.Init() && this.Columns != null && this.Columns.Count > 0;
        public IEnumerable<List<KeyValuePair<string, string>>> GetDataEntries()
        {
            for (string[] items = this._csvParser.Read(); items != null; items = this._csvParser.Read())
            {

                yield return this.BuildRow(items);
            }
        }

        private List<KeyValuePair<string, string>> BuildRow(string[] items)
        {
            if (items.Length != this.Columns.Count)
            {

            }
            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();
            for (int i = 0; i < this.Columns.Count; i++)
            {
                result.Add(new KeyValuePair<string, string>(Columns[i], items[i]));
            }
            return result;
        }
    }
}
