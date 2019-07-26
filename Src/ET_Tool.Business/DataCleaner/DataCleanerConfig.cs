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

        public DataCleanerConfig(string cleanerConfigFileName, IDiskIOHandler iOHandler)
        {
            this._cleanerConfigFileName = cleanerConfigFileName;
            this._iOHandler = iOHandler;
            this._cleanerConfigs = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, KeyValuePair<string, string>>>>(this._iOHandler.FileReadAllText(this._cleanerConfigFileName));
        }

        public Dictionary<string, KeyValuePair<string, string>> GetHeaderCleanConfigForSource(string attachedSourceName)
        {
            if (this._cleanerConfigs.ContainsKey(attachedSourceName) == false)
            {
                return new Dictionary<string, KeyValuePair<string, string>>();
            }
            return this._cleanerConfigs[attachedSourceName];
        }
    }
}