using System;

namespace ET_Tool.Business
{
    public interface IET_Actor: IDisposable
    {
        void Run();
        bool Init();
    }
}