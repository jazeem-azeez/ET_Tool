using System;

namespace ET_Tool.Business
{
    public interface IET_Engine : IDisposable
    {
        bool InitializePrepocessing();

        bool PerformTransformation();

        bool RunDataAnalysis(int attempt = 0);

        bool PerformAutoClean(string dataSourceFileName, string csvTypeDef, int attempt);

    }
}