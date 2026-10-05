using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows;
using System.Xml.Linq;

namespace TPLocalization
{
    public partial class MainWindow
    {
        public void ImportXML()
        {
            dt.Columns.Clear();
            dt.Rows.Clear();

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            if (ofd != null && ofd.FileName.Length > 1)
            {
                string fileText = File.ReadAllText(ofd.FileName);
                XDocument doc = XDocument.Parse(fileText);
                var datatables = doc.Descendants("DataTable");
                foreach (var datatable in datatables)
                {
                    dt.Rows.Add();
                    foreach (XElement child in datatable.Descendants())
                    {
                        string name = child.Name.ToString();
                        if (!dt.Columns.Contains(name))
                        {
                            dt.Columns.Add(name);
                        }
                    }
                }

                dt.ReadXml(ofd.FileName);

                UpdateDataGrid();
            }
        }

        public void ExportXML()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = ".xml";
            sfd.Filter = "Fichiers XML (*.xml)|*.xml";
            sfd.ShowDialog();
            if (sfd != null && sfd.FileName.Length > 1)
            {
                dt.WriteXml(sfd.FileName);
            }
        }
    }
}
