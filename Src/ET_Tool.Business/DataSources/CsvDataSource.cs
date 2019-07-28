using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;

using ET_Tool.Common;
using ET_Tool.Common.Logger;
using ET_Tool.Common.Models;

namespace ET_Tool.Business.DataSourceKinds
{
    public class CsvDataSource : CsvDataSourceKInd, IDataSource
    {
        public CsvDataSource(string sourceFileName, IDataCleaner dataCleaner, IEtLogger logger) : base(sourceFileName, dataCleaner, logger) => this.IsDataClean();

        public IEnumerable<DataCellCollection> GetDataRowEntries()
        {
            while (this._streamReader.EndOfStream == false)
            {
                yield return this.BuildRow(CsvParseHelper.GetAllFields(this._streamReader.ReadLine()));
            }
        }

        public string[] GetHeaders() => this.Columns.Select(c => c.Name).ToArray();

        public bool IsDataClean() => this.Init() && this.Columns != null && this.Columns.Count > 0;

        private DataCellCollection BuildRow(string[] items)
        {

            DataCellCollection row = new DataCellCollection();
            if (items.Length != this.Columns.Count)
            {
                this._logger.Log($"Columns & Values are not same {string.Join(",",items) }", EventLevel.Error, new DataMisalignedException("Columns & Values are not same"));
              //  return row;
            }
            for (int i = 0; i < this.Columns.Count && i < items.Length; i++)
            {
                row.Add(new DataCell(this.Columns[i], "", items[i]));
            }
            return this._dataCleaner.CleanRow(row);
        }
    }
}