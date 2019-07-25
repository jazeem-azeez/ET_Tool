using System;
using System.IO;
using ET_Tool.Business.Mappers;
using ET_Tool.Common.Logger;
using ET_Tool.IO;

namespace ET_Tool.Business
{
    public class ET_Engine : IET_Engine
    {
        private readonly IDataSourceFactory _dataSourceFactory;
        private readonly IDataMapAndFilterHandler _dataMapHandler;
        private readonly IDataSinkFactory _dataSinkFactory;
        private readonly IEtLogger _logger;
        private readonly IDiskIOHandler _diskIOHandler;
        private readonly RuntimeArgs _runtimeSettings;

        public ET_Engine(IDataSourceFactory dataSourceFactory,
                        IDataMapAndFilterHandler dataMapHandler,
                        IDataSinkFactory dataSinkFactory,
                        IEtLogger logger,
                        IDiskIOHandler diskIOHandler,
                        RuntimeArgs runtimeSettings)
        {
            this._dataSourceFactory = dataSourceFactory;
            this._dataMapHandler = dataMapHandler;
            this._dataSinkFactory = dataSinkFactory;
            this._logger = logger;
            this._diskIOHandler = diskIOHandler;
            this._runtimeSettings = runtimeSettings;
        }
        public void Dispose() => throw new NotImplementedException();
        public bool Init()
        {
            if (this._runtimeSettings.AutoDetectDataSources == true)
            {
                string[] sources = this._diskIOHandler.DirectoryGetFiles(Path.GetDirectoryName(this._runtimeSettings.SourceDataFolder), this._runtimeSettings.LookUpFilePattern, SearchOption.AllDirectories);
                foreach (string item in sources)
                {
                    using (IDataSource dataSource = this._dataSourceFactory.GetDataSource(item))
                    {
                        this._dataMapHandler.AddNewDataLookUp(Path.GetFileNameWithoutExtension(item), new DataLookUpCollection(dataSource, this._logger));
                    }
                }

            }

            return true;
        }
        public void Run()
        {

        }
    }
}
