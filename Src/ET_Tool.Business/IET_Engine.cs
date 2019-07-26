using System;

namespace ET_Tool.Business
{
    public interface IET_Engine : IDisposable
    {
        bool Init();

        void Run();
    }
}