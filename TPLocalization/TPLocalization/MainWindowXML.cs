using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Xml.Linq;

namespace TPLocalization
{
    public partial class MainWindow
    {
        public void ImportXML()
        {
            dt.Rows.Clear();
            dt.Columns.Clear();

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".xml";
            ofd.Filter = "Fichiers XML (*.xml)|*.xml";
            if (ofd.ShowDialog() == true && !string.IsNullOrEmpty(ofd.FileName))
            {
                string fileText = File.ReadAllText(ofd.FileName);
                XDocument doc = XDocument.Parse(fileText);
                var datatables = doc.Descendants("DataTable");

                foreach (var datatable in datatables)
                {
                    foreach (XElement child in datatable.Elements())
                    {
                        string name = child.Name.ToString();
                        if (!dt.Columns.Contains(name))
                        {
                            dt.Columns.Add(name);
                        }
                    }
                }

                int rowsAdded = 0;
                foreach (var datatable in datatables)
                {
                    DataRow newRow = dt.NewRow();

                    foreach (XElement child in datatable.Elements())
                    {
                        newRow[child.Name.ToString()] = child.Value;
                    }

                    dt.Rows.Add(newRow);
                    rowsAdded++;
                }

                UpdateDataGrid();
            }
        }


        public void ExportXML()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = ".xml";
            sfd.Filter = "Fichiers XML (*.xml)|*.xml";
            if (sfd.ShowDialog() == true && !string.IsNullOrEmpty(sfd.FileName))
            {
                dt.TableName = "DataTable";
                dt.WriteXml(sfd.FileName);
                MessageBox.Show("XML File exported!");
            }
        }
    }
}
