using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace TPLocalization
{
    public partial class MainWindow
    {
        private const string ConfigExtension = ".json";
        private const string ConfigFilterExtension = "Fichiers JSON (*.json)|*.json";

        public void ImportJSON()
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.DefaultExt = ConfigExtension;

            string content = "";

            if (ofd.ShowDialog() == true)
            {
                if (ofd.FileName.Length > 1)
                {
                    foreach (string file in ofd.FileNames)
                    {
                        content = File.ReadAllText(file, Encoding.UTF8);
                    }
                }
            }

            if (content != null)
            {
                DataTable importedData = JsonConvert.DeserializeObject<DataTable>(content);
                dt = importedData;
                MessageBox.Show("JSON Import done. :D");
            }
            else
            {
                MessageBox.Show("JSON Import failed. :-(");
            }
            UpdateDataGrid();
        }
        public void ExportJSON()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = ConfigExtension;
            sfd.Filter = ConfigFilterExtension;

            sfd.ShowDialog();
                MessageBox.Show($"Rows : {dt.Rows.Count}");

            string jsonString = JsonConvert.SerializeObject(dt);
            if (sfd != null && sfd.FileName.Length > 1)
            {

                File.WriteAllText(sfd.FileName, jsonString);
                MessageBox.Show("Export done.");
            }
            else
            {
                MessageBox.Show("Export failed. (I'm fcking bad :( )");
            }

        }
    }
}
