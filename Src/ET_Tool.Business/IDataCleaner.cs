using System.Collections.Generic;
using LumenWorks.Framework.IO.Csv;

namespace ET_Tool.Business
{
    public interface IDataCleaner
    { 
        void CleanHeader(List<Column> columns);
        List<KeyValuePair<string, string>> CleanRow(List<KeyValuePair<string, string>> result);
    }
}