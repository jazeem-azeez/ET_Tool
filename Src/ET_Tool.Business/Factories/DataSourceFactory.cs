using ET_Tool.Business.DataSourceKinds;
using ET_Tool.Common.Logger;

namespace ET_Tool.Business.Factories
{
    public class DataSourceFactory : IDataSourceFactory
    {
        private readonly IEtLogger _logger;

        public DataSourceFactory(IEtLogger etLogger) => this._logger = etLogger;

        public IDataSource GetDataSource(string name) => new CsvDataSource(name, this._logger);
    }
}