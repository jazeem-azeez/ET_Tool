using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using ET_Tool.Business.DataSink;
using ET_Tool.Business.DataSourceKinds;
using ET_Tool.Common.Logger;

using Microsoft.Extensions.Configuration;

namespace ET_Tool
{
    internal class Program
    {
        private static void Main(string[] args)
        {


            IConfigurationRoot configuration = new ConfigurationBuilder()
          .AddJsonFile("appsettings.json")
          .Build();

            EtLogger logger = new EtLogger(configuration, new Common.ConsoleIO.ConsoleProgressBar());

            logger.Log("Hello, Serilog!", EventLevel.LogAlways);
            CsvDataSource source = new CsvDataSource(@"E:\ET_Tool\Data\geo_unlocode\code-list.csv", logger);
            CsvDataSink csvDataSink = new CsvDataSink("out.csv", logger, "");
            csvDataSink.AddHeader(source.GetHeaders());

            //logger.ShowTable("csv", source.Columns.ToArray(), new List<string[]>(), false);
            foreach (List<KeyValuePair<string, string>> item in source.GetDataEntries())
            {
                csvDataSink.AddRow(item.Select(c => c.Value).ToArray());
                //logger.ShowRow(item.Select(c => c.Value).ToArray());
            }

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