using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ET_Tool.Common.Logger;

namespace ET_Tool.Business.Mappers
{
    public class SourceToSinkDataChainBuilder
    {
        public const string SourceKey = "Source";
        private readonly IEtLogger _logger;

        public SourceToSinkDataChainBuilder(IEtLogger logger)
        {
            this._logger = logger;
            this.Chain = new Dictionary<string, string>();
            this._steps = new Dictionary<string, Dictionary<string, string>>();
            this.SinkColumns = new HashSet<string>();
            this.SourceColumns = new HashSet<string>();
            this.LookUps = new Dictionary<string, HashSet<string>>();
        }

        public Dictionary<string, string> Chain { get; set; }
        private readonly Dictionary<string, Dictionary<string, string>> _steps;
        public Dictionary<string, HashSet<string>> LookUps { get; set; }
        public HashSet<string> SinkColumns { get; set; }
        public HashSet<string> SourceColumns { get; set; }

        public void AddSinkColumns(string[] columns)
        {
            foreach (string item in columns)
            {
                this.SinkColumns.Add(item);
            }
        }

        public void AddSourceColumns(string[] columns)
        {
            foreach (string item in columns)
            {
                this.SourceColumns.Add(item);
            }
        }

        public void BuildChain()
        {
            string value = string.Empty;
            bool hasResolved;
            int lenght = 0;
            foreach (string sinkItem in this.SinkColumns)
            {
             //   this._logger.ProgressBar(lenght++, this.SinkColumns.Count);

                hasResolved = false;

                if (this.SourceColumns.Contains(sinkItem))
                {
                    value = Path.Combine(SourceKey, sinkItem);
                    this._steps.Add(sinkItem, new Dictionary<string, string>() { { SourceKey, sinkItem } });
                    this.Chain.Add(sinkItem, value);
                    this._logger.LogInformation($"Found chain {value}");
                    continue;
                }
                //currently only supports two level of lookup
                foreach (KeyValuePair<string, HashSet<string>> lkUpEntry in this.LookUps)
                {
                    if (lkUpEntry.Value.Contains(sinkItem))
                    {
                        foreach (string srcItem in this.SourceColumns)
                        {
                            if (lkUpEntry.Value.Contains(srcItem))
                            {
                                value = Path.Combine(SourceKey, srcItem, lkUpEntry.Key, sinkItem);

                                this._steps.Add(sinkItem, new Dictionary<string, string>() { { SourceKey, srcItem }, { lkUpEntry.Key, sinkItem } });
                                this.Chain.Add(sinkItem, value);
                                this._logger.LogInformation($"Found chain {value}");
                                hasResolved = true;
                                break;
                            }
                        }

                        if (hasResolved == false)
                        {
                            this._logger.Log($"Failed to Properly Resolve {sinkItem}", System.Diagnostics.Tracing.EventLevel.Error);
                        }
                    }
                }
            }

            this._logger.LogInformation("Source to destination Chain building completed");
            this._logger.ShowTable("Source to destination Data Chain", new string[] { "SourceColumns", "Path" }, this.Chain.Select(link => new string[] { link.Key, link.Value }).ToList(), true);

            foreach (string item in this.SinkColumns)
            {
                if (this.Chain.ContainsKey(item) == false)
                {
                    this._logger.Log("Build Chain Integrity Check Failed", System.Diagnostics.Tracing.EventLevel.Error);
                    throw new Exception("Data dependency Chain build failed ");
                }
            }


            this._logger.Log("Build Chain Integrity Check Passed", System.Diagnostics.Tracing.EventLevel.Informational);
        }

        public Dictionary<string, string> GetSteps(string name) => this._steps[name];
    }
}