using System;

namespace ET_Tool.Business
{
    public interface IET_Engine: IDisposable
    {
        void Run();
        bool Init();
    }
}