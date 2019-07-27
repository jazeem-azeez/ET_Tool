using System;
using System.Collections.Generic;
using System.Text;
using LumenWorks.Framework.IO.Csv;

namespace ET_Tool.Common.Models
{
    public class DataCellCollection
    {
        private string[] columns;

        public DataCellCollection()
        {
            Cells = new List<DataCell>();
        }

      

        public List<DataCell> Cells { get; set; }

        public void Add(DataCell dataCell) => Cells.Add(dataCell);
    }
}
