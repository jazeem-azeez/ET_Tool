using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Tracing;
using System.Linq;

using ET_Tool.Common.Logger;
using ET_Tool.Common.Models;

namespace ET_Tool.Business.Mappers
{
    public class DataLookUpCollection : IDataLookUpCollection
    {
        private readonly DataTable _dataTable;
        private readonly Dictionary<string, string> _dictionaryLookup;
        private readonly IEtLogger _logger;
        private readonly bool useDataTable;

        public DataLookUpCollection(IDataSource dataSource, IEtLogger logger)
        {
            this._logger = logger;
            this._dataTable = new DataTable();
            this.useDataTable = false;
            this._dictionaryLookup = new Dictionary<string, string>();

            this.Columns = dataSource.GetHeaders();
            if (this.Columns.Length > 2)
            {
                foreach (string item in this.Columns)
                {
                    this._dataTable.Columns.Add(item);
                }
                this.useDataTable = false;
            }
            foreach (DataCellCollection item in dataSource.GetDataRowEntries())
            {
                string[] row = item.Cells.Select(c => c.Value).ToArray();
                if (this.useDataTable)
                {
                    this._dataTable.Rows.Add(row);
                }
                else
                {
                    this._dictionaryLookup.Add(row[0], row[1]);
                }
            }
        }

        public string[] Columns { get; private set; }

        public string LookUp(string keyColumn, string valueColumn, string key) => (this.useDataTable) ? this.DataTableLookUp(keyColumn, valueColumn, key) : this.DictionaryLookUp(key);

        private string DataTableLookUp(string keyColumnName, string valueColumnName, string key)
        {
            if (this._dataTable.Columns.Contains(valueColumnName)&& this._dataTable.Columns.Contains(keyColumnName))
            {
                int indexOfOutValueCol = this._dataTable.Columns[valueColumnName].Ordinal;
                DataRow[] row = this._dataTable.Select(string.Format("{0} = '{1}'", keyColumnName, key.Replace("'", "''")));

                if (row != null && row.Length > 0)
                {
                    return row[0].ItemArray[indexOfOutValueCol].ToString();
                }
                this._logger.Log("Value Not found in Lookup", EventLevel.Warning);
                return string.Empty;
            }
            this._logger.Log("Column Not found in Lookup", EventLevel.Error);
            return string.Empty;
        }

        private string DictionaryLookUp(string key)
        {
            if (this._dictionaryLookup.ContainsKey(key) == false)
            {
                this._logger.Log($"Value for {key} Not found in Lookup", EventLevel.Warning);
                return string.Empty;
            }
            else
            {
                return this._dictionaryLookup[key];
            }
        }
    }
}