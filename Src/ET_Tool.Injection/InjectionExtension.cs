using System.Collections.Generic;
using ET_Tool.Business;
using ET_Tool.Business.DataCleaner;
using ET_Tool.Business.Factories;
using ET_Tool.Business.Mappers;
using ET_Tool.Business.Mappers.Transformation;
using ET_Tool.Common.IO;
using ET_Tool.Common.IO.ConsoleIO;
using ET_Tool.Common.Logger;
using Microsoft.Extensions.DependencyInjection;

namespace ET_Tool.Injection
{
    public static class InjectionExtension
    {
        public static IServiceCollection MainInjection(this IServiceCollection services)
        {
            services.AddSingleton<IDiskIOHandler, DiskIOHandler>();
            services.AddSingleton<IEtLogger, EtLogger>();
            services.AddSingleton((ctx) => new ConsoleProgressBar());
            services.AddSingleton<IDataCleanerFactory, DataCleanerFactory>();
            services.AddSingleton<IDataSourceFactory, DataSourceFactory>();
            services.AddSingleton<IDataSinkFactory, DataSinkFactory>();
            services.AddTransient<IDataResolver, DataResolver>();
            services.AddTransient<IDataCleanerConfig, DataCleanerConfig>();
            services.AddTransient<IET_Engine, ET_Engine>();
            services.AddTransient((ctx =>
            {
                DegreeToDecimalLatLongMapper latlongMapper = new DegreeToDecimalLatLongMapper(ctx.GetRequiredService<RuntimeArgs>());

                return new Dictionary<string, IDataMapper>() { { latlongMapper.Name, latlongMapper } }; ;
            }));
            services.AddTransient((ctx =>
            {
                return new Dictionary<string, IDataFilter>();
            }));
            return services;
        }
    }
}
