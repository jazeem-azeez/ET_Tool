using System.Collections.Generic;

namespace ET_Tool.Business.Mappers.Transformation
{
    internal class DegreeToDecimalLatLongMapper : IDataMapper
    {
        private const string latitudeKey = "LatitudeKey";
        private const string longitudeKey = "LongitudeKey";
        private readonly RuntimeArgs _runtimeArgs;
        private const string columnkey = "ColumnKey";

        public DegreeToDecimalLatLongMapper(RuntimeArgs runtimeArgs) => this._runtimeArgs = runtimeArgs;

        public string Name => "DegreeToDecimalLatLongMapper";

        public void BindToLookUpCollection(Dictionary<string, DataLookUpCollection> globalLookUpCollection)
        {
            // NOP method
        }

        public HashSet<string> GetAssociatedColumns() => new HashSet<string>(
            new string[]
          {
              this._runtimeArgs.DegreeToDecimalLatLongMapperSettings[nameof(columnkey)],
              this._runtimeArgs.DegreeToDecimalLatLongMapperSettings[nameof(latitudeKey)],
              this._runtimeArgs.DegreeToDecimalLatLongMapperSettings[nameof(longitudeKey)],
          });

        public List<KeyValuePair<string, string>> Map(List<KeyValuePair<string, string>> mappingContextValues,
                                                        string columnkey,
                                                        string value,
                                                        List<KeyValuePair<string, string>> resultList)
        {
            if (
                columnkey != this._runtimeArgs.DegreeToDecimalLatLongMapperSettings[nameof(columnkey)] ||
                resultList.Exists(item => item.Key == this._runtimeArgs.DegreeToDecimalLatLongMapperSettings[latitudeKey]) ||
                resultList.Exists(item => item.Key == this._runtimeArgs.DegreeToDecimalLatLongMapperSettings[longitudeKey])
                )
            {
                return resultList;
            }

            List<KeyValuePair<string, string>> temp = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>(columnkey, value), new KeyValuePair<string, string>(columnkey + 1, value) };
            return temp;
        }
    }
}