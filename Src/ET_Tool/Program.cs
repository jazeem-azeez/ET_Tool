using System;
using System.Diagnostics.Tracing;
using ET_Tool.Business;
using ET_Tool.Business.DataCleaner;
using ET_Tool.Common.IO;
using ET_Tool.Common.IO.ConsoleIO;
using ET_Tool.Common.Logger;
using ET_Tool.Injection;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace ET_Tool
{
    internal class Program
    {
        private static void Main(string[] args)
        {


            EtLogger logger = new EtLogger(new ConsoleProgressBar());

            logger.Log("=> Starting Custom CSV Et_Tool : ", EventLevel.LogAlways);
            IDiskIOHandler diskIOHandler = new DiskIOHandler();
            RuntimeArgs runtimeSettings = JsonConvert.DeserializeObject<RuntimeArgs>(diskIOHandler.FileReadAllText("runtimeConfig.Json"));
            
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton(runtimeSettings);
            services.MainInjection();
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            IET_Engine engine = serviceProvider.GetRequiredService<IET_Engine>();
            if (engine.RunDataAnalysis() && engine.InitializePrepocessing())
            {
                engine.PerformTransformation();
            }
            logger.LogInformation("Press Enter to Exit");
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