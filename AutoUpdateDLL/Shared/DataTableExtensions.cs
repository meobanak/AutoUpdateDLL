using System;
using System.API;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace System
{
    public static class DataTableExtensions
    {
        public static IEnumerable<T> Convert<T>(this DataTable table)
        {
            List<T> data = new List<T>();

            var type = typeof(T);
            var fields = type.GetFields();

            foreach (DataRow row in table.Rows)
            {
                var item = (T)type.GetConstructor(Type.EmptyTypes).Invoke(null);

                foreach (var field in fields)
                {
                    var value = row[field.Name];
                    if (value == DBNull.Value)
                        value = null;

                    if (field.FieldType == typeof(int))
                    {
                        field.SetValue(item, System.Convert.ToInt32(value));
                    }
                    else if (field.FieldType == typeof(bool))
                    {
                        field.SetValue(item, System.Convert.ToBoolean(value));
                    }
                    else
                    {
                        field.SetValue(item, value);
                    }
                }

                data.Add(item);
            }

            return data;
        }



        public static DataTable Delete(this DataTable table, string filter)
        {
            if (table == null)
                return null;

            table.Select(filter).Delete();
            return table;
        }
        public static void Delete(this IEnumerable<DataRow> rows)
        {
            foreach (var row in rows)
                row.Delete();
        }




        public static string GetSplitAllData(this DataTable dt, string colName, string Delimiter)
        {
            string s = "";

            var dr = dt.Select();
            if (dr.Length == 0)
            {
                return s;
            }

            for (int i = 0; i < dr.Length; i++)
            {
                var row = dr[i];
                if (row != null)
                {
                    if (i == 0)
                    {
                        s += row.Item(colName);
                    }
                    else
                    {
                        s += Delimiter + row.Item(colName);
                    }
                }
            }
            return s;
        }


        public static string GetSplit(this DataTable dt, string colName, string Delimiter, string criteria)
        {
            string s = "";

            var dr = dt.Select(criteria);
            if (dr.Length == 0)
            {
                return s;
            }

            for (int i = 0; i < dr.Length; i++)
            {
                var row = dr[i];
                if (row != null)
                {
                    if (i == 0)
                    {
                        s += row.Item(colName);
                    }
                    else
                    {
                        s += Delimiter + row.Item(colName);
                    }
                }
            }
            return s;
        }
        /// <summary>
        /// như hàm cũ, chỉ khác là đổi qua double cho giá trị tìm được
        /// </summary>
        /// <param name="dt">datatable</param>
        /// <param name="colName">tên cột</param>
        /// <param name="Delimiter">dấu cách</param>
        /// <param name="criteria">điều kiện tìm</param>
        /// <returns></returns>
        public static string GetSplitDouble(this DataTable dt, string colName, string Delimiter, string criteria)
        {
            string s = "";
            CultureInfo ci = new CultureInfo("en-US");
            var dr = dt.Select(criteria);
            if (dr.Length == 0)
            {
                return s;
            }

            for (int i = 0; i < dr.Length; i++)
            {
                var row = dr[i];
                if (row != null)
                {
                    if (i == 0)
                    {
                        s += row.Item(colName).Replace(",", ".");
                    }
                    else
                    {
                        s += Delimiter + row.Item(colName).Replace(",", ".");
                    }
                }
            }
            return s;
        }

        public static DataTable MergeTable(this List<DataTable> listTable)
        {
            if (listTable.Count == 0)
            {
                return null;
            }
            var table_final = listTable[0];

            for (int i = 1; i < listTable.Count; i++)
            {
                if (listTable[i].Rows.Count == 0)
                    break;
                foreach (DataRow dr in listTable[i].Rows)
                {
                    DataRow newRow = table_final.NewRow();
                    newRow.ItemArray = dr.ItemArray;

                    table_final.Rows.Add(newRow);
                }
            }
            return table_final;
        }

        public static string DataTableToJSON(this DataTable Dt)
        {
            string[] StrDc = new string[Dt.Columns.Count];

            string HeadStr = string.Empty;
            for (int i = 0; i < Dt.Columns.Count; i++)
            {
                StrDc[i] = Dt.Columns[i].Caption;
                HeadStr += "\"" + StrDc[i] + "\":\"" + StrDc[i] + i.ToString() + "¾" + "\",";
            }

            HeadStr = HeadStr.Substring(0, HeadStr.Length - 1);

            StringBuilder Sb = new StringBuilder();

            Sb.Append("[");

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                string TempStr = HeadStr;

                for (int j = 0; j < Dt.Columns.Count; j++)
                {
                    TempStr = TempStr.Replace(Dt.Columns[j] + j.ToString() + "¾", Dt.Rows[i][j].ToString().Trim());
                }
                //Sb.AppendFormat("{{{0}}},",TempStr);

                Sb.Append("{" + TempStr + "},");
            }

            Sb = new StringBuilder(Sb.ToString().Substring(0, Sb.ToString().Length - 1));

            if (Sb.ToString().Length > 0)
                Sb.Append("]");

            return StripControlChars(Sb.ToString());
        }

        public static string DataTableToJSON(this DataTable Dt, List<string> list)
        {
            string[] StrDc = new string[Dt.Columns.Count];

            string HeadStr = string.Empty;
            for (int i = 0; i < Dt.Columns.Count; i++)
            {
                StrDc[i] = Dt.Columns[i].Caption;
                HeadStr += "\"" + StrDc[i] + "\":\"" + StrDc[i] + i.ToString() + "¾" + "\",";
            }

            HeadStr = HeadStr.Substring(0, HeadStr.Length - 1);

            StringBuilder Sb = new StringBuilder();

            Sb.Append("[");

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                string TempStr = HeadStr;

                for (int j = 0; j < Dt.Columns.Count; j++)
                {
                    //TempStr = TempStr.Replace(Dt.Columns[j] + j.ToString() + "¾", Dt.Rows[i][j].ToString().Trim());

                    TempStr = TempStr.Replace(Dt.Columns[j] + j.ToString() + "¾", Dt.Rows[i][j].ToString().Trim());
                }
                //Sb.AppendFormat("{{{0}}},",TempStr);

                Sb.Append("{" + TempStr + "},");
            }

            Sb = new StringBuilder(Sb.ToString().Substring(0, Sb.ToString().Length - 1));

            if (Sb.ToString().Length > 0)
                Sb.Append("]");

            return StripControlChars(Sb.ToString());
        }

        //To strip control characters:

        //A character that does not represent a printable character but //serves to initiate a particular action.

        public static string StripControlChars(string s)
        {
            return Regex.Replace(s, @"[^\x20-\x7F]", "");
        }

        public static DataTable MergeAll(this IList<DataTable> tables, String primaryKeyColumn)
        {
            if (!tables.Any())
                throw new ArgumentException("Tables must not be empty", "tables");
            if (primaryKeyColumn != null)
                foreach (DataTable t in tables)
                    if (!t.Columns.Contains(primaryKeyColumn))
                        throw new ArgumentException("All tables must have the specified primarykey column " + primaryKeyColumn, "primaryKeyColumn");

            if (tables.Count == 1)
                return tables[0];

            DataTable table = new DataTable("TblUnion");
            table.BeginLoadData(); // Turns off notifications, index maintenance, and constraints while loading data
            foreach (DataTable t in tables)
            {
                table.Merge(t); // same as table.Merge(t, false, MissingSchemaAction.Add);
            }
            table.EndLoadData();

            if (primaryKeyColumn != null)
            {
                // since we might have no real primary keys defined, the rows now might have repeating fields
                // so now we're going to "join" these rows ...
                var pkGroups = table.AsEnumerable()
                    .GroupBy(r => r[primaryKeyColumn]);
                var dupGroups = pkGroups.Where(g => g.Count() > 1);
                foreach (var grpDup in dupGroups)
                {
                    // use first row and modify it
                    DataRow firstRow = grpDup.First();
                    foreach (DataColumn c in table.Columns)
                    {
                        if (firstRow.IsNull(c))
                        {
                            DataRow firstNotNullRow = grpDup.Skip(1).FirstOrDefault(r => !r.IsNull(c));
                            if (firstNotNullRow != null)
                                firstRow[c] = firstNotNullRow[c];
                        }
                    }
                    // remove all but first row
                    var rowsToRemove = grpDup.Skip(1);
                    foreach (DataRow rowToRemove in rowsToRemove)
                        table.Rows.Remove(rowToRemove);
                }
            }

            return table;
        }


        public static DataTable DataColumnToTable(this DataColumnCollection collector)
        {
            DataTable temp = new DataTable();
            temp.Columns.Add("Name");

            for (int i = 0; i < collector.Count; i++)
            {
                var item = collector[i];
                temp.Rows.Add(item.ColumnName);
            }
            return temp;
        }

        public static DataTable Init(this DataTable table)
        {
            table = new DataTable();
            table.Columns.Add("CaseID");
            table.Columns.Add("AutomationID");
            table.Columns.Add("MissingCounting", typeof(int));

            return table;
        }
    }
}
