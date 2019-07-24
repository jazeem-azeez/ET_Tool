using System;
using System.Collections.Generic;
using System.Text;
using ET_Tool.Common.Logger;

namespace ET_Tool.Business
{
    public class ET_Actor : IET_Actor
    {
        private readonly IIngestActor _ingestActor;
        private readonly IEnumerable<ILookupActor> _lookupActors;
        private readonly IEnumerable<ITransformationActors> _transformationActors;
        private readonly IEtLogger _etLogger;

        public ET_Actor(IIngestActor ingestActor, IEnumerable<ILookupActor> lookupActors, IEnumerable<ITransformationActors> transformationActors, IEtLogger etLogger)
        {
            this._ingestActor = ingestActor;
            this._lookupActors = lookupActors;
            this._transformationActors = transformationActors;
            this._etLogger = etLogger;
        }
        public void Dispose() => throw new NotImplementedException();
        public bool Init() => throw new NotImplementedException();
        public void Run()
        {

        }
    }
}
