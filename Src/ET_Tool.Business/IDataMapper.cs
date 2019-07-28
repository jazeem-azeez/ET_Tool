using System.Collections.Generic;

using ET_Tool.Business.Mappers;
using ET_Tool.Common.Models;

namespace ET_Tool.Business
{
    public interface IDataMapper
    {
        string Name { get; }

        void BindToLookUpCollection(Dictionary<string, DataLookUpCollection> globalLookUpCollection);

        List<DataCell> Map(DataCellCollection sourceRow, string columnkey, string value,  Dictionary<string, string> Context, DataCellCollection currentState);
        HashSet<string> GetAssociatedColumns();
    }
}