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
        private const string ConfigSchemaExtension = ".schema";

        public void ImportJSON()
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.DefaultExt = ConfigExtension;
            ofd.Filter = ConfigFilterExtension;

            string content = "";

            if (ofd.ShowDialog() == true && !string.IsNullOrEmpty(ofd.FileName))
            {
                if (ofd.FileName.Length > 1)
                {
                    foreach (string file in ofd.FileNames)
                    {
                        content = File.ReadAllText(file, Encoding.UTF8);
                    }
                }
            }

            if (!string.IsNullOrEmpty(ofd.FileName))
            {
                DataTable importedData = JsonConvert.DeserializeObject<DataTable>(content);
                dt = importedData;
            }
            else
            {
                MessageBox.Show("JSON Import failed. :-(");
                return;
            }
            UpdateDataGrid();

            MessageBox.Show("JSON Import done. :D");
        }
        public void ExportJSON()
        {
            //Dictionary<string, string> 

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = ConfigExtension;
            sfd.Filter = ConfigFilterExtension;

            sfd.ShowDialog();
            //MessageBox.Show($"Rows : {dt.Rows.Count}");

            string jsonString = JsonConvert.SerializeObject(dt);
            if (sfd != null && sfd.FileName.Length > 1)
            {
                string schemaPath = ConfigSchemaExtension + jsonString;

                File.WriteAllText(sfd.FileName, jsonString);
            }
            else
            {
                MessageBox.Show("Export failed.");
                return;
            }
            MessageBox.Show("Export done.");
        }
    }
}
