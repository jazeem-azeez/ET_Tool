using System.Collections.Generic;

namespace ET_Tool.Business
{
    public interface IDataSource
    {
        IEnumerable <List<KeyValuePair<string, string>>> GetDataEntries();
        string[] GetHeaders(); 
        bool IsDataClean();
    }
}