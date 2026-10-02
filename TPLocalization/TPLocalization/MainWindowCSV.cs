using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows;

namespace TPLocalization
{
    public partial class MainWindow
    {
        public void InportCSV(string _st)
        {
            string[] e = _st.Split(",");
        }

        public void ExportCSV()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                sb.Append(dt.Columns[i].ColumnName);
                if (i < dt.Columns.Count - 1)
                    sb.Append(";");
            }
            sb.AppendLine();

            foreach (DataRow row in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    string value;

                    // Format DateTime columns as "yyyy-MM-dd"
                    if (dt.Columns[i].DataType == typeof(DateTime))
                    {
                        value = ((DateTime)row[i]).ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        value = row[i].ToString();
                    }

                    // Escape special characters: commas, quotes, newlines
                    if (value.Contains(";") || value.Contains("\"") || value.Contains("\n"))
                    {
                        value = "\"" + value.Replace("\"", "\"\"") + "\"";
                    }

                    sb.Append(value);

                    if (i < dt.Columns.Count - 1) sb.Append(";");
                }
                sb.AppendLine();
            }
            string filepath = "fileCSV.csv";
            File.WriteAllText(filepath, sb.ToString(), Encoding.UTF8);
        }
    }
}
