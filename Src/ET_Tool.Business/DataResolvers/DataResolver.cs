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
        private readonly Dictionary<string, DataLookUpCollection> _globalLookUpCollection = new Dictionary<string, DataLookUpCollection>();

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


        public void AddNewDataLookUp(string key, DataLookUpCollection lookUpCollection) => this._globalLookUpCollection.Add(key, lookUpCollection);



        //public List<KeyValuePair<string, string>> FilterDataFor(string columnkey, string value, List<KeyValuePair<string, string>> mappingContextValues)
        //{
        //    List<KeyValuePair<string, string>> resultList = new List<KeyValuePair<string, string>>();
        //    if (this._mappingRulesCollection.ContainsKey(columnkey))
        //    {
        //        foreach (string item in this._mappingRulesCollection[columnkey])
        //        {
        //            IDataMapper mapper = this.GetMapper(item);
        //            resultList = mapper.Map(mappingContextValues, columnkey, value, resultList);
        //        }
        //    }
        //    else
        //    {
        //        resultList.Add(new KeyValuePair<string, string>(columnkey, value));
        //    }
        //    return resultList;
        //}

        //public List<KeyValuePair<string, string>> MapDataFor(string columnkey, string value, List<KeyValuePair<string, string>> mappingContextValues)
        //{
        //    List<KeyValuePair<string, string>> resultList = new List<KeyValuePair<string, string>>();
        //    if (this._mappingRulesCollection.ContainsKey(columnkey))
        //    {
        //        foreach (string item in this._mappingRulesCollection[columnkey])
        //        {
        //            IDataMapper mapper = this.GetMapper(item);
        //            resultList = mapper.Map(mappingContextValues, columnkey, value, resultList);
        //        }
        //    }
        //    else
        //    {
        //        resultList.Add(new KeyValuePair<string, string>(columnkey, value));
        //    }
        //    return resultList;
        //}

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

            foreach (KeyValuePair<string, string> item in steps)
            {


                if (this._dataMappers.ContainsKey(item.Key))
                {

                    outRowCollection = this._dataMappers[item.Key].Map(currentColumn.Name, currVal, context, SourceRow); 

                }


                //TODOD move to constants
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
                    if (_mappingRulesCollection.ContainsKey(item.Key))
                    {
                        foreach (var mappingRule in _mappingRulesCollection[item.Key])
                        {
                            switch (mappingRule)
                            {
                                case "column-is-csv-lookup":
                                default:
                                    break;
                            }
                        }
                    }
                    currVal = this._globalLookUpCollection[item.Key].LookUp(item.Key, item.Value, currVal);
                }
                //if (currentColumn.Name == item.Value)
                //{
                //    outRowCollection.Cells.Add(new DataCell(new Column { Name = item.Key }, "", currVal));
                //}
            }
            outRowCollection.Cells.Add(new DataCell(new Column { Name = currentColumn.Name }, "", currVal));

            return outRowCollection;
        }
    }
}