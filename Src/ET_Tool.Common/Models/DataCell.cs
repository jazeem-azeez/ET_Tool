using System;
using System.Collections.Generic;
using System.Text;
using LumenWorks.Framework.IO.Csv;

namespace ET_Tool.Common.Models
{
    public class DataCell
    {
        public DataCell(Column column, string RowKey, string Value)
        {
            this.Column = column;
            this.RowKey = RowKey;
            this.Value = Value;
        }

        public Column Column { get; set; }
        public string RowKey { get; set; }
        public string  Value { get; set; }
    }
}
