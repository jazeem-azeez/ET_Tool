using System.Collections.Generic;
using System.Text.RegularExpressions;

using ET_Tool.Common.Models;

namespace ET_Tool.Business.Mappers.Transformation
{
    public class DegreeToDecimalLatLongMapper : IDataMapper
    {

        private readonly RuntimeArgs _runtimeArgs;

        public DegreeToDecimalLatLongMapper(RuntimeArgs runtimeArgs) => this._runtimeArgs = runtimeArgs;

        public string Name => "DegreeToDecimalLatLongMapper";

        public void BindToLookUpCollection(Dictionary<string, DataLookUpCollection> globalLookUpCollection)
        {
            // NOP method
        }

        public double ConvertDegreeAngleToDouble(string point)
        {
            int multiplier = (point.Contains("S") || point.Contains("W")) ? -1 : 1; //handle south and west

            point = Regex.Replace(point, "[^0-9.]", "");
            string mins = point.Substring(point.Length - 2, 2);
            string degs = point.Substring(0, point.Length - 2);

            double degrees = double.Parse(degs);
            double minutes = double.Parse(mins) / 60;

            return (degrees + minutes) * multiplier;
        }

        public HashSet<string> GetAssociatedColumns() => new HashSet<string>(
            new string[]
  {
              this._runtimeArgs.DegreeToDecimalLatLongMapperSettings[Constants.columnkey],
              this._runtimeArgs.DegreeToDecimalLatLongMapperSettings[Constants.latitudeKey],
              this._runtimeArgs.DegreeToDecimalLatLongMapperSettings[Constants.longitudeKey],
  });

        public List<KeyValuePair<string, string>> Map(string columnkey,
            string value,
            List<KeyValuePair<string, string>> mappingContextValues,
            List<KeyValuePair<string, string>> currentState)
        {
            if (
                columnkey != this._runtimeArgs.DegreeToDecimalLatLongMapperSettings[Constants.columnkey] ||
                currentState.Exists(item => item.Key == this._runtimeArgs.DegreeToDecimalLatLongMapperSettings[Constants.latitudeKey]) ||
                currentState.Exists(item => item.Key == this._runtimeArgs.DegreeToDecimalLatLongMapperSettings[Constants.longitudeKey])
                )
            {
                return currentState;
            }

            List<KeyValuePair<string, string>> temp = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>(columnkey, value), new KeyValuePair<string, string>(columnkey + 1, value) };
            return temp;
        }

        public DataCellCollection Map(string columnkey, string value, Dictionary<string, string> Context, DataCellCollection currentState)
        {
            if (
              columnkey != this._runtimeArgs.DegreeToDecimalLatLongMapperSettings[Constants.columnkey] ||
              currentState.Cells.Exists(item => item.Column.Name == this._runtimeArgs.DegreeToDecimalLatLongMapperSettings[Constants.latitudeKey]) ||
              currentState.Cells.Exists(item => item.Column.Name == this._runtimeArgs.DegreeToDecimalLatLongMapperSettings[Constants.longitudeKey])
              )
            {
                return currentState;
            }

            string[] latlong = value.Split(" ");
            if (latlong.Length <= 0)
            {
                return currentState;
            }

            double latitude = latlong[0].Contains('N') || latlong[0].Contains('S') ? this.ConvertDegreeAngleToDouble(latlong[0]) : this.ConvertDegreeAngleToDouble(latlong[1]);
            double longitude = latlong[0].Contains('E') || latlong[0].Contains('W') ? this.ConvertDegreeAngleToDouble(latlong[0]) : this.ConvertDegreeAngleToDouble(latlong[1]);

            currentState.Add(new DataCell(new LumenWorks.Framework.IO.Csv.Column() { Name = this._runtimeArgs.DegreeToDecimalLatLongMapperSettings[Constants.latitudeKey] }, "", latitude.ToString()));
            currentState.Add(new DataCell(new LumenWorks.Framework.IO.Csv.Column() { Name = this._runtimeArgs.DegreeToDecimalLatLongMapperSettings[Constants.longitudeKey] }, "", longitude.ToString()));

            return currentState;
        }
    }
}