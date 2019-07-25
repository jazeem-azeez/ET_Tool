using System.Collections.Generic;
using ET_Tool.Business.Mappers;

namespace ET_Tool.Business
{
    public interface IDataMapper
    {
        List<KeyValuePair<string, string>> Map(List<KeyValuePair<string, string>> mappingContextValues, string columnkey, string value, List<KeyValuePair<string, string>> resultList );
        void BindToLookUpCollection(Dictionary<string, DataLookUpCollection> globalLookUpCollection);
    }
}