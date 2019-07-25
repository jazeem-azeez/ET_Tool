using System;

namespace ET_Tool.Business
{
    public interface IDataSink:IDisposable
    {
        void LoadOutpuConfiguration();
        void Initialize();
        void AddRecordsToSink(string[] values);
        string[] Columns { get; }
    }
}