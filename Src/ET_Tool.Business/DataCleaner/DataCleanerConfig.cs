using System.Collections.Generic;

using ET_Tool.Common;
using ET_Tool.Common.IO;
using Newtonsoft.Json;

namespace ET_Tool.Business.DataCleaner
{
    public class DataCleanerConfig : IDataCleanerConfig
    {
        private readonly string _cleanerConfigFileName;
        private readonly Dictionary<string, Dictionary<string, KeyValuePair<string, string>>> _cleanerConfigs;
        private readonly IDiskIOHandler _iOHandler;

        public DataCleanerConfig(RuntimeArgs runtimeArgs, IDiskIOHandler iOHandler)
        {
            this._cleanerConfigFileName = runtimeArgs.DefaultCleanerConfig;
            this._iOHandler = iOHandler;
            this._cleanerConfigs = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, KeyValuePair<string, string>>>>(this._iOHandler.FileReadAllText(this._cleanerConfigFileName));
        }

        public Dictionary<string, KeyValuePair<string, string>> GetCleanConfigForSource(string attachedSourceName,string part)
        {
            string key = $"{attachedSourceName}-{ part}";
            if (this._cleanerConfigs.ContainsKey(key) == false)
            {
                return new Dictionary<string, KeyValuePair<string, string>>();
            }
            return this._cleanerConfigs[key];
        }
        public Dictionary<string, KeyValuePair<string, string>> GetRowCleanConfigForSource(string attachedSourceName) => GetCleanConfigForSource(attachedSourceName, "row");
        public Dictionary<string, KeyValuePair<string, string>> GetHeaderCleanConfigForSource(string attachedSourceName) => GetCleanConfigForSource(attachedSourceName, "header");
    }
}