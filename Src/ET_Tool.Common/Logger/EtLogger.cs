using System;
using System.Diagnostics.Tracing;

using Microsoft.Extensions.Configuration;

using Serilog;

namespace ET_Tool.Common.Logger
{
    public class EtLogger : IEtLogger
    {
        private readonly Serilog.Core.Logger _slogger;

        public EtLogger(IConfigurationRoot configuration)
        {
            LoggerConfiguration loggerConfiguration = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.ColoredConsole()
            .WriteTo.File("log.txt",
                rollingInterval: RollingInterval.Hour,
                rollOnFileSizeLimit: true);

            this._slogger = loggerConfiguration.CreateLogger(); ;
        }

        public void Log(string message, EventLevel eventLevel, Exception exception = null)
        {
            switch (eventLevel)
            {
                case EventLevel.LogAlways:
                    this._slogger.Information(message);
                    break;

                case EventLevel.Critical:
                    this._slogger.Fatal(exception, message);
                    break;

                case EventLevel.Error:
                    this._slogger.Error(exception, message);
                    break;

                case EventLevel.Warning:
                    this._slogger.Warning(message);
                    break;

                case EventLevel.Informational:
                    this._slogger.Verbose(message);
                    break;

                case EventLevel.Verbose:
                    this._slogger.Debug(message);
                    break;

                default:
                    break;
            }
        }
    }
}