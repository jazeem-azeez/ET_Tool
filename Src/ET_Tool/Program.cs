using System;
using System.Diagnostics.Tracing;
using ET_Tool.Common.Logger;

using Microsoft.Extensions.Configuration;
using Serilog;

namespace ET_Tool
{
    internal class Program
    {
        private static void Main(string[] args)
        {
           

            var configuration = new ConfigurationBuilder()
          .AddJsonFile("appsettings.json")
          .Build();

            var Logger = new EtLogger(configuration);

            Logger.Log("Hello, Serilog!",EventLevel.LogAlways); 
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