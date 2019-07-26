using System.Collections.Generic;

namespace ET_Tool.Business
{
    public interface IDataCleanerConfig
    {
        Dictionary<string, KeyValuePair<string, string>> GetHeaderCleanConfigForSource(string attachedSourceName);
    }
}