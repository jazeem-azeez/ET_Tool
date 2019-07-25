using System;
using System.Collections.Generic;
using System.Text;
using ET_Tool.Common.Logger;

namespace ET_Tool.Business
{
    public class ET_Actor : IET_Actor
    {
        private readonly IDataSource _dataSource;
        private readonly IEnumerable<IDataMapper> _dataMappers;
        private readonly IEnumerable<IDataFilter> _dataFilters;
        private readonly IDataSink _dataSink;
        private readonly IEtLogger _etLogger;

        public ET_Actor(IDataSource dataSource, IEnumerable<IDataMapper> dataMappers, IEnumerable<IDataFilter> dataFilters, IDataSink dataSink, IEtLogger etLogger)
        {
            this._dataSource = dataSource;
            this._dataMappers = dataMappers;
            this._dataFilters = dataFilters;
            this._dataSink = dataSink;
            this._etLogger = etLogger;
        }
        public void Dispose() => throw new NotImplementedException();
        public bool Init() => throw new NotImplementedException();
        public void Run()
        {

        }
    }
}
