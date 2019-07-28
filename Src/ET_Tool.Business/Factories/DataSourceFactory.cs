using ET_Tool.Business.DataSourceKinds;
using ET_Tool.Common.Logger;

namespace ET_Tool.Business.Factories
{
    public class DataSourceFactory : IDataSourceFactory
    {
        private readonly IEtLogger _logger;
        private readonly IDataCleanerFactory _dataCleanerFactory;

        public DataSourceFactory(IEtLogger etLogger, IDataCleanerFactory dataCleanerFactory )
        {
            this._logger = etLogger;
            this._dataCleanerFactory = dataCleanerFactory;
        }

        public IDataSource GetDataSource(string name) => new CsvDataSource(name, this._dataCleanerFactory.GetDataCleaner(name),this._logger);
    }
}