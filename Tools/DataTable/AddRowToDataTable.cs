using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SRLCore.Tools
{
    public partial class DataTableTools
    {
        public static void AddRowToDataTable(DataTable dt, Dictionary<string, object> item)
        {
            DataRow row = dt.NewRow();

            foreach (var i in item)
            {
                row[i.Key] = i.Value;
            }

            dt.Rows.Add(row); 
        }
    }
}
