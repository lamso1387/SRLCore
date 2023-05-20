using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SRLCore.Tools
{
    public partial class JsonTools
    {

        public static DataTable ConvertJsonToDataTable(List<Dictionary<string, object>> list)
        {
            DataTable dt = new DataTable();

            bool create_columns = false;

            int all_ = list.Count;

            foreach (var item in list)
            {
                Dictionary<string, object> item_to_show = new Dictionary<string, object>();

                foreach (var item_ in item)
                {
                    if (item_.Value != null) item_to_show[item_.Key.ToString()] = item_.Value.ToString();
                    else item_to_show[item_.Key.ToString()] = null;
                }

                if (!create_columns)
                    foreach (var col in item_to_show.Keys)
                    {
                        dt.Columns.Add(col);
                        create_columns = true;
                    }
                List<string> columns = new List<string>();
                foreach (DataColumn col in dt.Columns)
                {
                    columns.Add(col.ColumnName);
                }
                List<string> new_col = item_to_show.Keys.Where(x => !columns.Contains(x)).ToList();
                foreach (var i in new_col)
                {
                    dt.Columns.Add(i.ToString());
                }

                Tools.DataTableTools.AddRowToDataTable(dt, item_to_show);
            }

            return dt;

        }

     

    }
}
