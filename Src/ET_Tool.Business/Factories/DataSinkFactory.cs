using ET_Tool.Business.DataSink;
using ET_Tool.Common.Logger;

namespace ET_Tool.Business.Factories
{
    public class DataSinkFactory : IDataSinkFactory
    {
        private readonly IEtLogger _logger;

        public DataSinkFactory(IEtLogger logger) => this._logger = logger;
        public IDataSink GetDataSink(string name, string outConfiguration) => new CsvDataSink(name, this._logger, outConfiguration);
    }
}