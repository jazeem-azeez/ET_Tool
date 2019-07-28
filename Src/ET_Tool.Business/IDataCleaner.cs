using System.Collections.Generic;
using ET_Tool.Common.Models;
using LumenWorks.Framework.IO.Csv;

namespace ET_Tool.Business
{
    public interface IDataCleaner
    { 
        void CleanHeader(List<Column> columns);
        DataCellCollection CleanRow(DataCellCollection result);
    }
}