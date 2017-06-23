using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Util
{
    public class ConvertHelper
    {
        public static DataTable ListToDataTable<T>(List<T> list)
        {
            DataTable dt = new DataTable(typeof(T).Name);
            foreach (PropertyInfo info in typeof(T).GetProperties())
            {
                if (info.PropertyType == typeof(int?))
                    dt.Columns.Add(new DataColumn(info.Name, typeof(int)));
                else if (info.PropertyType == typeof(DateTime?))
                    dt.Columns.Add(new DataColumn(info.Name, typeof(DateTime)));
                else if (info.PropertyType == typeof(decimal?))
                    dt.Columns.Add(new DataColumn(info.Name, typeof(decimal)));
                else if (info.PropertyType == typeof(double?))
                    dt.Columns.Add(new DataColumn(info.Name, typeof(double)));
                else
                    dt.Columns.Add(new DataColumn(info.Name, info.PropertyType));
            }
            foreach (T t in list)
            {
                DataRow row = dt.NewRow();
                foreach (PropertyInfo info in typeof(T).GetProperties())
                {
                    row[info.Name] = info.GetValue(t, null) == null ? DBNull.Value : info.GetValue(t, null);
                }
                dt.Rows.Add(row);
            }
            return dt;
        }
    }
}