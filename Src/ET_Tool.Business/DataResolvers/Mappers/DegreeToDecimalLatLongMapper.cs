using System;
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

        public List<DataCell> Map(DataCellCollection sourceRows, string columnkey, string value, Dictionary<string, string> Context, DataCellCollection currentState)
        {
            List<DataCell> result = new List<DataCell>();



            string[] latlong = value.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (latlong == null || latlong.Length < 2)
            {
                result.Add(new DataCell(new LumenWorks.Framework.IO.Csv.Column() { Name = columnkey }, "", string.Empty));
                return result;
            }

            double latitude = latlong[0].Contains('N') || latlong[0].Contains('S') ? this.ConvertDegreeAngleToDouble(latlong[0]) : this.ConvertDegreeAngleToDouble(latlong[1]);
            double longitude = latlong[0].Contains('E') || latlong[0].Contains('W') ? this.ConvertDegreeAngleToDouble(latlong[0]) : this.ConvertDegreeAngleToDouble(latlong[1]);
            if (columnkey == this._runtimeArgs.DegreeToDecimalLatLongMapperSettings[Constants.latitudeKey])
            {
                result.Add(new DataCell(new LumenWorks.Framework.IO.Csv.Column() { Name = this._runtimeArgs.DegreeToDecimalLatLongMapperSettings[Constants.latitudeKey] }, "", latitude.ToString()));
            }
            if (columnkey == this._runtimeArgs.DegreeToDecimalLatLongMapperSettings[Constants.longitudeKey])
            {
                result.Add(new DataCell(new LumenWorks.Framework.IO.Csv.Column() { Name = this._runtimeArgs.DegreeToDecimalLatLongMapperSettings[Constants.longitudeKey] }, "", longitude.ToString()));
            }

            return result;
        }
    }
}