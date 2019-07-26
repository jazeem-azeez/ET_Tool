using System.Collections.Generic;
using System.IO;

using ET_Tool.Common.Logger;

namespace ET_Tool.Business.Mappers
{
    public class SourceToSinkDataChainBuilder
    {
        private const string SourceKey = "Source";
        private readonly IEtLogger _logger;

        public SourceToSinkDataChainBuilder(IEtLogger logger)
        {
            this._logger = logger;
            this.Chain = new Dictionary<string, string>();
            this.SinkColumns = new HashSet<string>();
            this.SourceColumns = new HashSet<string>();
            this.LookUps = new Dictionary<string, HashSet<string>>();
        }

        public Dictionary<string, string> Chain { get; set; }
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
            bool hasResolved ;
            foreach (string sinkItem in this.SinkColumns)
            {
                hasResolved = false;

                if (this.SourceColumns.Contains(sinkItem))
                {
                    value = Path.Combine(SourceKey, $".{sinkItem}");
                    this.Chain.Add(sinkItem, value);
                    this._logger.LogInformation($"Found chain {value}");
                    continue;
                }
                //currently only supports one level of lookup
                foreach (KeyValuePair<string, HashSet<string>> lkUpEntry in this.LookUps)
                {
                    //string []values = entry.Value.Split(',');
                    if (lkUpEntry.Value.Contains(sinkItem))
                    {
                        foreach (string srcItem in this.SourceColumns)
                        {
                            if (lkUpEntry.Value.Contains(srcItem))
                            {
                                value = Path.Combine(SourceKey, $".{srcItem}", lkUpEntry.Key, $".{sinkItem}");
                                this.Chain.Add(sinkItem, value);
                                this._logger.LogInformation($"Found chain {value}");
                                hasResolved = true;
                                break;
                            }
                        }

                        if (hasResolved == false)
                        {
                            _logger.Log($"Failed to Properly Resolve {sinkItem}", System.Diagnostics.Tracing.EventLevel.Error);
                        }
                    }
                }
            }
        }
    }
}