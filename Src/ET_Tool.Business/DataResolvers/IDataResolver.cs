using System.Collections.Generic;

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
    }
}