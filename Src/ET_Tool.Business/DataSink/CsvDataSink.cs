using System.Collections.Generic;
using ET_Tool.Business.DataSink.DataSinkKinds;
using ET_Tool.Common.Logger;
using ET_Tool.Common.Models;

namespace ET_Tool.Business.DataSink
{
    public class CsvDataSink : CsvDataSinkKind, IDataSink
    {
        public CsvDataSink(string outputFileName, IEtLogger etLogger, string outConfigurationFile) : base(outputFileName, etLogger, outConfigurationFile)
        {
        }

    }
}