using System.Collections.Generic;
using LumenWorks.Framework.IO.Csv;

namespace ET_Tool.Business.DataCleaner
{
    public class CsvDataCleaner : IDataCleaner
    {
        private readonly string _attachedSourceName;
        private readonly IDataCleanerConfig _dataCleanerConfig;

        public CsvDataCleaner(string attachedSourceName, IDataCleanerConfig dataCleanerConfig)
        {
            this._attachedSourceName = attachedSourceName;
            this._dataCleanerConfig = dataCleanerConfig;
        }
        public void CleanHeader(List<Column> columns)
        {
            Dictionary<string, KeyValuePair<string, string>> cfg = this._dataCleanerConfig.GetHeaderCleanConfigForSource(this._attachedSourceName);
            if (cfg.Count > 0)
            {
                foreach (Column item in columns)
                {
                    if (cfg.ContainsKey(item.Name))
                    {
                        switch (cfg[item.Name].Key)
                        {
                            case "rename": item.Name = cfg[item.Name].Value; break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
        public List<KeyValuePair<string, string>> CleanRow(List<KeyValuePair<string, string>> result) => result;
    }
}
