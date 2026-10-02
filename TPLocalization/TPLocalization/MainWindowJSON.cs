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
        private const string SchemaExtension = ".schema.json";

        public void ImportJSON(string _fileName)
        {

            string content = File.ReadAllText(_fileName, Encoding.UTF8);

            DataTable importedData = JsonConvert.DeserializeObject<DataTable>(content);

            dt = importedData;

            datagrid.ItemsSource = dt.DefaultView;
        }
        public void ExportJSON()
        {

            string jsonString = JsonConvert.SerializeObject(dt);
            File.WriteAllText("TPLocalization" + ConfigExtension, jsonString);

            MessageBox.Show("Export done.");
        }
    }
}
