using System;
using System.Collections.Generic;
using ET_Tool.Common.Models;

namespace ET_Tool.Business
{
    public interface IDataSink : IDisposable
    {
        string[] Columns { get; }

        void AddRecordsToSink(string[] values);

        void Initialize();

        void LoadOutpuConfiguration();
        void AddRecordsToSink(List<DataCell> cells);
    }
}