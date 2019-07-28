using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using ET_Tool.Business;
using ET_Tool.Business.DataCleaner;
using ET_Tool.Business.Factories;
using ET_Tool.Business.Mappers;
using ET_Tool.Business.Mappers.Transformation;
using ET_Tool.Common.IO;
using ET_Tool.Common.IO.ConsoleIO;
using ET_Tool.Common.Logger;
using ET_Tool.Common.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ET_Tool
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
          .AddJsonFile("appsettings.json")
          .Build();

            EtLogger logger = new EtLogger(configuration, new ConsoleProgressBar());

            logger.Log("Hello, Serilog!", EventLevel.LogAlways);

            IDiskIOHandler diskIOHandler = new DiskIOHandler();
            RuntimeArgs runtimeSettings = new RuntimeArgs()
            {
                AutoBuild = true,
                DataSinkFileName = @"out.csv",
                DataSourceFileName = @"E:\ET_Tool\Data\geo_unlocode\code-list.csv",
                DegreeToDecimalLatLongMapperSettings = new Dictionary<string, string>
                {
                    { Constants.columnkey, "Coordinates" },
                    { Constants.latitudeKey, "Latitude" },
                    { Constants.longitudeKey, "Longitude" }
                },
                LookUpFilePattern = "*.txt",
                OutConfigFileName = "outConfig.json",
                SourceDataFolder = @"E:\ET_Tool\Data\geo_unlocode\",
                DefaultCleanerConfig ="cleanerConfig.json",
                MappingRulesSourcePath = "mappingRules.json"

            };
            diskIOHandler.FileWriteAllText("runtimeConfig.Json", JsonConvert.SerializeObject(runtimeSettings));
            IDataCleanerConfig cleanerConfig = new DataCleanerConfig(runtimeSettings.DefaultCleanerConfig, diskIOHandler);
            IDataCleanerFactory cleanerFactory = new DataCleanerFactory(cleanerConfig);
            IDataSourceFactory dataSourceFactory = new DataSourceFactory(logger, cleanerFactory);
            IDataSinkFactory dataSinkFactory = new DataSinkFactory(logger);
            DegreeToDecimalLatLongMapper degreeToDecimalLatLongMapper = new DegreeToDecimalLatLongMapper(runtimeSettings);
            Dictionary<string, IDataMapper> dataMappers = new Dictionary<string, IDataMapper>() { { degreeToDecimalLatLongMapper.Name, degreeToDecimalLatLongMapper } };
            Dictionary<string, IDataFilter> dataFilter = new Dictionary<string, IDataFilter>();
            IDataResolver dataResolver = new DataResolver(dataMappers, dataFilter, logger);
            ET_Engine engine = new ET_Engine(dataSourceFactory, dataResolver, dataSinkFactory, logger, diskIOHandler, runtimeSettings);

            engine.Init();
            engine.RunDataAnalysis();
            engine.PerformTransformation();


            //logger.ShowTable("csv", source.Columns.ToArray(), new List<string[]>(), false);
            //foreach (List<KeyValuePair<string, string>> item in source.GetDataRowEntries())
            //{
            //    csvDataSink.AddRecordsToSink(item.Select(c => c.Value).ToArray());
            //    logger.ShowRow(item.Select(c => c.Value).ToArray());
            //}

            //Logger.CloseAndFlush();


            Console.ReadLine();
            /*
             * 1. Load Configurations
             *      Get Emit Configuration
             *          [
             *          destination type
             *          destination colums
             *          ]
             *      Get Ingest Configuration
             *          [
             *          Load Main Data Source
             *          Load Lookups
             *          Load Transformation Lookups
             *          ]
             *
             * 2. Run Extraction Phase
             * 3. Run Transformation Phase
             * 4. Emit OutPut
             * 5. CheckSum Output , Input
             *
             */
        }
    }
}