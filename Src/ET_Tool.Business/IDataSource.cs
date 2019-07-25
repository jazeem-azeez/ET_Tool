using System;
using System.Collections.Generic;

namespace ET_Tool.Business
{
    public interface IDataSource:IDisposable
    {
        IEnumerable <List<KeyValuePair<string, string>>> GetDataRowEntries();
        string[] GetHeaders(); 
        bool IsDataClean();
    }
}