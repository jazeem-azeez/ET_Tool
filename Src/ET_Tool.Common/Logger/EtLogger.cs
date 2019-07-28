using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;

using ET_Tool.Common.IO.ConsoleIO;

using Microsoft.Extensions.Configuration;

//using Console = Colorful.Console;
using Serilog;

namespace ET_Tool.Common.Logger
{
    public class EtLogger : IEtLogger
    {
        private const int tableWidth = 100;
        private readonly ConsoleProgressBar _progressBar;
        private readonly Serilog.Core.Logger _slogger;

        public EtLogger(IConfigurationRoot configuration, ConsoleProgressBar progressBar)
        {
            LoggerConfiguration loggerConfiguration = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("log.txt", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug,
                rollingInterval: RollingInterval.Hour,
                rollOnFileSizeLimit: true)
            .WriteTo.ColoredConsole(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information);
            this._slogger = loggerConfiguration.CreateLogger();
            this._progressBar = progressBar;
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

        public void LogError(string errormessage, Exception exception=null) =>this.Log(errormessage,EventLevel.Error,exception==null?new Exception(errormessage):exception);
        public void LogInformation(string message) => this.Log(message, EventLevel.LogAlways);

        public void ProgressBar(int progress, int total, int level = -1) => this._progressBar.DrawTextProgressBar(progress, total, level);

        public void ShowRow(string[] rows) => this.PrintRow(rows);

        public void ShowTable(string TableName, string[] headers, List<string[]> rows, bool closeAtEnd = true)
        {
            this.PrintLine();
            this.PrintRow(new string[] { TableName });
            this.PrintLine();
            this.PrintRow(headers);
            this.PrintLine();

            foreach (string[] item in rows)
            {
                this.PrintRow(item.ToArray());
            }
            if (closeAtEnd == true)
            {
                this.PrintLine();
            }
        }

        private string AlignCentre(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
        }

        private string PrintLine(int width = tableWidth)
        {
            string temp = new string('-', width);
            Console.WriteLine(temp);
            return temp;
        }

        private string PrintRow(params string[] columns)
        {
            if (columns == null || columns.Length == 0)
            {
                return this.PrintLine();
            }
            int width = (tableWidth - columns.Length) / columns.Length;
            string row = "|";

            foreach (string column in columns)
            {
                row += this.AlignCentre(column, width) + "|";
            }

            Console.WriteLine(row);
            this.Log(row, EventLevel.Informational);
            return row;
        }
    }
}