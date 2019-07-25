using System;
using System.Collections.Generic;
using System.Text;
using ET_Tool.Business.DataSink.DataSinkKinds;
using ET_Tool.Common.Logger;

namespace ET_Tool.Business.DataSink
{
    public class CsvDataSink : CsvDataSinkKind
    {
        public CsvDataSink(string destFileName, IEtLogger etLogger, string destTemplateConfigurationFile) : base(destFileName, etLogger, destTemplateConfigurationFile)
        {
        }

    }
}
