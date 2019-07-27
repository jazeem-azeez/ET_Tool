﻿using System;
using System.Collections.Generic;
using System.IO;

using ET_Tool.Business.Mappers;
using ET_Tool.Common.IO;
using ET_Tool.Common.Logger;
using ET_Tool.Common.Models;
using LumenWorks.Framework.IO.Csv;

namespace ET_Tool.Business
{
    public class ET_Engine : IET_Engine
    {
        private readonly IDataResolver _dataMapHandler;
        private readonly IDataSinkFactory _dataSinkFactory;
        private readonly IDataSourceFactory _dataSourceFactory;
        private readonly IDiskIOHandler _diskIOHandler;
        private readonly IEtLogger _logger;
        private readonly RuntimeArgs _runtimeSettings;
        private readonly SourceToSinkDataChainBuilder _toSinkDataChainBuilder;

        public ET_Engine(IDataSourceFactory dataSourceFactory,
                        IDataResolver dataMapHandler,
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
            if (this._runtimeSettings.AutoBuild == true)
            {
                string[] sources = this._diskIOHandler.DirectoryGetFiles(Path.GetDirectoryName(this._runtimeSettings.SourceDataFolder), this._runtimeSettings.LookUpFilePattern, SearchOption.AllDirectories);
                foreach (string item in sources)
                {
                    using (IDataSource dataSource = this._dataSourceFactory.GetDataSource(item))
                    {
                        DataLookUpCollection lookUpCollection = new DataLookUpCollection(dataSource, this._logger);
                        string key = Path.GetFileName(item);
                        this._toSinkDataChainBuilder.LookUps.Add(key, new HashSet<string>(dataSource.GetHeaders()));
                        this._dataMapHandler.AddNewDataLookUp(key, lookUpCollection);
                    }
                }
                foreach (IDataMapper item in this._dataMapHandler.GetAllMappers())
                {
                    this._toSinkDataChainBuilder.LookUps.Add(item.Name, item.GetAssociatedColumns());
                }
                using (IDataSource dataSource = this._dataSourceFactory.GetDataSource(this._runtimeSettings.DataSourceFileName))
                {
                    this._toSinkDataChainBuilder.AddSourceColumns(dataSource.GetHeaders());

                }
                using (IDataSink dataSink = this._dataSinkFactory.GetDataSink(this._runtimeSettings.DataSinkFileName, this._runtimeSettings.OutConfigFileName))
                {
                    this._toSinkDataChainBuilder.AddSinkColumns(dataSink.Columns);

                }

                this._toSinkDataChainBuilder.BuildChain();
            }

            return true;
        }

        public void Run()
        {
            using (IDataSource dataSource = this._dataSourceFactory.GetDataSource(this._runtimeSettings.DataSourceFileName))
            {
                using (IDataSink dataSink = this._dataSinkFactory.GetDataSink(this._runtimeSettings.DataSinkFileName, this._runtimeSettings.OutConfigFileName))
                {
                    string[] rowValues = new string[dataSink.Columns.Length];
                    Dictionary<string, string> context = new Dictionary<string, string>();
                    foreach (DataCellCollection row in dataSource.GetDataRowEntries())
                    {
                        DataCellCollection outRowCollection = new DataCellCollection();
                        for (int i = 0; i < dataSink.Columns.Length; i++)
                        {
                            Dictionary<string, string> steps = this._toSinkDataChainBuilder.GetSteps(dataSink.Columns[i]);

                            outRowCollection = this._dataMapHandler.Resolve(row, new Column() { Name = dataSink.Columns[i] }, outRowCollection, steps, context);

                        }

                        dataSink.AddRecordsToSink(outRowCollection.Cells);
                    }
                }
            }
        }


    }
}