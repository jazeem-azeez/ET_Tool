using System;
using System.Collections.Generic;
using System.Text;
using ET_Tool.Business.DataCleaner;

namespace ET_Tool.Business.Factories
{
    public class DataCleanerFactory : IDataCleanerFactory
    {
        private readonly IDataCleanerConfig _cleanerConfig;

        public DataCleanerFactory(IDataCleanerConfig cleanerConfig)
        {
            this._cleanerConfig = cleanerConfig;
        }
        public IDataCleaner GetDataCleaner(string name, IDataCleanerConfig cleanerConfig) => new CsvDataCleaner(name, cleanerConfig);
        public IDataCleaner GetDataCleaner(string name) => GetDataCleaner(name, _cleanerConfig);
    }
}
