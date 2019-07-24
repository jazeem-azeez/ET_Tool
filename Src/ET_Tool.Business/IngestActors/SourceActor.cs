using System;
using ET_Tool.Common;
using ET_Tool.Common.Logger;

namespace ET_Tool.Business.IngestActor
{
    public class SourceActor : CsvSourceActorBase, IIngestActor
    {
        public SourceActor(string sourceFileName, IEtLogger logger) : base(sourceFileName, logger)
        {

        } 


    }
}
