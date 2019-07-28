using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ET_Tool.Common.Models;

using LumenWorks.Framework.IO.Csv;

namespace ET_Tool.Business.DataCleaner
{
    public class CsvDataCleaner : IDataCleaner
    {
        private readonly string _attachedSourceName;
        private readonly IDataCleanerConfig _dataCleanerConfig;
        private readonly Dictionary<string, KeyValuePair<string, string>> headerCleanCfg;
        private readonly Dictionary<string, KeyValuePair<string, string>> rowCleanCfg;

        public CsvDataCleaner(string attachedSourceName, IDataCleanerConfig dataCleanerConfig)
        {
            this._attachedSourceName = attachedSourceName;
            this._dataCleanerConfig = dataCleanerConfig;
            this.headerCleanCfg = this._dataCleanerConfig.GetHeaderCleanConfigForSource(Path.GetFileName(this._attachedSourceName));
            this.rowCleanCfg = this._dataCleanerConfig.GetRowCleanConfigForSource(Path.GetFileName(this._attachedSourceName));
        }

        public void CleanHeader(List<Column> columns)
        {
            if (this.headerCleanCfg.Count > 0)
            {
                foreach (Column item in columns)
                {
                    if (this.headerCleanCfg.ContainsKey(item.Name))
                    {
                        switch (this.headerCleanCfg[item.Name].Key)
                        {
                            case "rename":
                                string newName = this.headerCleanCfg[item.Name].Value;
                                if (this.rowCleanCfg.ContainsKey(item.Name))
                                {
                                    KeyValuePair<string, KeyValuePair<string, string>> newEntry = new KeyValuePair<string, KeyValuePair<string, string>>(newName, this.rowCleanCfg[item.Name]);
                                    this.rowCleanCfg.Remove(item.Name);
                                    this.rowCleanCfg.Add(newEntry.Key, newEntry.Value);
                                }
                                item.Name = newName;
                                break;

                            default:
                                break;
                        }
                    }
                }
            }
        }

        public DataCellCollection CleanRow(DataCellCollection result)
        {
            if (this.rowCleanCfg.Count > 0)
            {
                foreach (DataCell item in result.Cells)
                {
                    if (this.rowCleanCfg.ContainsKey(item.Column.Name))
                    {
                        string colName = item.Column.Name;
                        switch (this.rowCleanCfg[colName].Key)
                        {
                            case "filter-for-symbol":
                                Regex regEx = new Regex(this.rowCleanCfg[colName].Value);
                                MatchCollection matches = regEx.Matches(item.Value);
                                item.Value = string.Join("", matches.Select(m => m.Value).ToArray());
                                break;
                            case "remove-symbols":
                                string[] symbols = this.rowCleanCfg[colName].Value.Split(' ');
                                for (int i = 0; i < symbols.Length; i++)
                                {
                                    item.Value = item.Value.Replace(symbols[i], "");
                                }
                                break;

                            case "replace-symbol":
                                symbols = this.rowCleanCfg[colName].Value.Split(' ');
                                item.Value = item.Value.Replace(symbols[0], symbols[1]);

                                break;

                            default: break;
                        }
                    }
                }
            }
            return result;
        }
    }
}