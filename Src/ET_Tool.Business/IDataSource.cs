using System;
using System.Collections.Generic;
using ET_Tool.Common.Models;

namespace ET_Tool.Business
{
    public interface IDataSource : IDisposable
    {
        IEnumerable<DataCellCollection> GetDataRowEntries();

        string[] GetHeaders();

        bool IsDataClean();
    }
}