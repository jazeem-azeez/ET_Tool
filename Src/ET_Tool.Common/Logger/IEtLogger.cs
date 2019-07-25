using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;

namespace ET_Tool.Common.Logger
{
    public interface IEtLogger
    {
        void Log(string message, EventLevel eventLevel, Exception exception = null);

        void ProgressBar(int progress, int Total, int level = -1);
        void ShowTable(string TableName, string[] headers, List<string[]> rows, bool closeAtEnd = true);
        void ShowRow(string[] rows);

    }
}
