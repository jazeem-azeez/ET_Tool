using System;
using System.Collections.Generic;
using System.Linq;

using ET_Tool.Common.Logger;
using ET_Tool.Common.Models;

namespace ET_Tool.Business.DataSourceKinds
{
    public class CsvDataSource : CsvDataSourceKInd, IDataSource
    {
        public CsvDataSource(string sourceFileName, IDataCleaner dataCleaner, IEtLogger logger) : base(sourceFileName, dataCleaner, logger) => this.IsDataClean();

        public IEnumerable<DataCellCollection> GetDataRowEntries()
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

        private DataCellCollection BuildRow(string[] items)
        {
            if (items.Length != this.Columns.Count)
            {
                throw new DataMisalignedException("Columns & Values are not same");
            }
            DataCellCollection row = new DataCellCollection();
            for (int i = 0; i < this.Columns.Count; i++)
            {
                row.Add(new DataCell(Columns[i], "", items[i]));
            }
            return _dataCleaner.CleanRow(row);
        }
    }
}