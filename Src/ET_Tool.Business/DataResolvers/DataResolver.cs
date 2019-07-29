using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;

using ET_Tool.Common.Logger;
using ET_Tool.Common.Models;
using LumenWorks.Framework.IO.Csv;

namespace ET_Tool.Business.Mappers
{
    public class DataResolver : IDataResolver
    {
        private readonly Dictionary<string, IDataFilter> _dataFilter;
        private readonly Dictionary<string, IDataMapper> _dataMappers;
        private readonly Dictionary<string, List<string>> _filterRulesCollection = new Dictionary<string, List<string>>();
        private readonly IEtLogger _logger;
        private readonly Dictionary<string, List<string>> _mappingRulesCollection = new Dictionary<string, List<string>>();
        private readonly Dictionary<string, IDataLookUpCollection> _globalLookUpCollection = new Dictionary<string, IDataLookUpCollection>();

        public DataResolver(Dictionary<string, IDataMapper> dataMappers, Dictionary<string, IDataFilter> dataFilter, IEtLogger logger)
        {
            this._dataMappers = dataMappers;
            this._dataFilter = dataFilter;
            foreach (KeyValuePair<string, IDataMapper> item in dataMappers)
            {
                item.Value.BindToLookUpCollection(this._globalLookUpCollection);
            }
            this._logger = logger;
        }

        public IEnumerable<IDataMapper> GetAllMappers() => this._dataMappers.Values;
        public IEnumerable<IDataFilter> GetAllFilter() => this._dataFilter.Values;


        public void AddNewDataLookUp(string key, IDataLookUpCollection lookUpCollection) => this._globalLookUpCollection.Add(key, lookUpCollection);
        public void AddNewMappingRule(string key, List<string> rules) => this._mappingRulesCollection.Add(key, rules);



        private IDataMapper GetMapper(string item)
        {
            if (this._dataMappers.ContainsKey(item))
            {
                return this._dataMappers[item];
            }
            else
            {
                MissingMemberException exception = new MissingMemberException($"Missing Mapper {item}");
                this._logger.Log("Missing or Invalid Mapper", EventLevel.Error, exception);
                throw exception;
            }
        }

        public DataCellCollection Resolve(DataCellCollection SourceRow, Column currentColumn, DataCellCollection outRowCollection, Dictionary<string, string> steps, Dictionary<string, string> context)
        {

            string currVal = string.Empty;
            string peviousCol = string.Empty;
            string peviousSource = string.Empty;
            List<DataCell> dataCells = new List<DataCell>();
            string destColumn = currentColumn.Name;
            KeyValuePair<string, string> prevItem = new KeyValuePair<string, string>();
            foreach (KeyValuePair<string, string> item in steps)
            {


                if (this._dataMappers.ContainsKey(item.Key))
                {
                    dataCells.AddRange(this._dataMappers[item.Key].Map(SourceRow, currentColumn.Name, currVal, context, outRowCollection));
                }


                if (item.Key == Constants.SourceColKey)
                {
                    for (int i = 0; i < SourceRow.Cells.Count; i++)
                    {
                        if (item.Value == SourceRow.Cells[i].Column.Name)
                        {
                            currVal = SourceRow.Cells[i].Value;
                            break;
                        }
                    }
                }
                if (this._globalLookUpCollection.ContainsKey(item.Key))
                {
                    if (this._mappingRulesCollection.ContainsKey(GetMappingRuleKey(item, prevItem)))
                    {
                        currVal = this.ApplyMappingRule(currVal, item, prevItem, (mappingItem) => this._globalLookUpCollection[item.Key].LookUp(item.Key, item.Value, mappingItem));
                    }
                    else
                    {
                        currVal = this._globalLookUpCollection[item.Key].LookUp(item.Key, item.Value, currVal);
                    }
                }

                //if (currentColumn.Name == item.Value)
                //{
                //    outRowCollection.Cells.Add(new DataCell(new Column { Name = item.Key }, "", currVal));
                //}
                prevItem = item;
            }

            if (dataCells.Count == 0)
            {
                outRowCollection.Cells.Add(new DataCell(new Column { Name = currentColumn.Name }, "", currVal));
            }
            else
            {
                outRowCollection.Cells.AddRange(dataCells);
            }

            return outRowCollection;
        }

        private string ApplyMappingRule(string currVal, KeyValuePair<string, string> currdestPair, KeyValuePair<string, string> sourcePair, Func<string, string> callBack)
        {
            foreach (string mappingRule in this._mappingRulesCollection[GetMappingRuleKey(currdestPair, sourcePair)])
            {
                switch (mappingRule)
                {
                    case "split-by-letters":
                        List<string> accumCollection = new List<string>();
                        foreach (var csvItem in currVal)
                        {
                            accumCollection.Add(callBack($"{csvItem}"));
                        }
                        currVal = string.Join(",", accumCollection);
                        break;
                    default:
                        break;
                }
            }

            return currVal;
        }

        private static string GetMappingRuleKey(KeyValuePair<string, string> currdestPair, KeyValuePair<string, string> sourcePair) => $"{sourcePair.Key}:{sourcePair.Value}=>{currdestPair.Key}:{currdestPair.Value}";
    }
}