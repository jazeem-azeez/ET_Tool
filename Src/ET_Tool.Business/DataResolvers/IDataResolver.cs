using System.Collections.Generic;
using ET_Tool.Common.Models;
using LumenWorks.Framework.IO.Csv;

namespace ET_Tool.Business.Mappers
{
    public interface IDataResolver
    {
        void AddNewDataLookUp(string key, DataLookUpCollection lookUpCollection);

        //void AddNewDataMapRule(string key, List<string> mappers);

        //List<KeyValuePair<string, string>> FilterDataFor(string columnkey, string value, List<KeyValuePair<string, string>> contextValues);

        //List<KeyValuePair<string, string>> MapDataFor(string columnkey, string value, List<KeyValuePair<string, string>> contextValues);
        IEnumerable<IDataMapper> GetAllMappers();
        IEnumerable<IDataFilter> GetAllFilter();
        DataCellCollection Resolve(DataCellCollection SourceRow, Column currentColumn, DataCellCollection outRowCollection, Dictionary<string, string> steps, Dictionary<string, string> context);
    }
}