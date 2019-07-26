using System;

namespace ET_Tool.Business
{
    public interface IDataSink : IDisposable
    {
        string[] Columns { get; }

        void AddRecordsToSink(string[] values);

        void Initialize();

        void LoadOutpuConfiguration();
    }
}