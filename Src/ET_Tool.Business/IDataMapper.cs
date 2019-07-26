using System.Collections.Generic;

using ET_Tool.Business.Mappers;

namespace ET_Tool.Business
{
    public interface IDataMapper
    {
        string Name { get; }

        void BindToLookUpCollection(Dictionary<string, DataLookUpCollection> globalLookUpCollection);

        List<KeyValuePair<string, string>> Map(List<KeyValuePair<string, string>> mappingContextValues, string columnkey, string value, List<KeyValuePair<string, string>> resultList);
        HashSet<string> GetAssociatedColumns();
    }
}