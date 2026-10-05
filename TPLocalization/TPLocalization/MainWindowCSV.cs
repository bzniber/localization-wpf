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
        public void InportCSV()
        {
            dt.Columns.Clear();
            dt.Rows.Clear();

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();

            if(ofd != null && ofd.FileName.Length > 1)
            {
                string[] lignes = File.ReadAllLines(ofd.FileName, Encoding.UTF8);

                if (lignes.Length > 0)
                {
                    // Création des colonnes à partir de la première ligne
                    string[] enTetes = lignes[0].Split(';');
                    foreach (string enTete in enTetes)
                    {
                        dt.Columns.Add(enTete.Trim());
                    }

                    // Ajout des lignes de données
                    for (int i = 1; i < lignes.Length; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(lignes[i]))
                        {
                            string[] valeurs = lignes[i].Split(';');
                            dt.Rows.Add(valeurs);
                        }
                    }
                }
            }
            UpdateDataGrid();
        }

        public void ExportCSV()
        {
            SaveFileDialog save = new SaveFileDialog();
            save.DefaultExt = ".csv";
            save.Filter = "Fichiers CSV (*.csv)|*.csv";

            if (save.ShowDialog() == true)
            {
                StringBuilder sb = new StringBuilder();

                // Récupération des en-têtes de colonnes
                string[] colonnes = dt.Columns.Cast<DataColumn>()
                    .Select(c => c.ColumnName).ToArray();
                sb.AppendLine(string.Join(";", colonnes));

                // Récupération des lignes de données
                foreach (DataRow row in dt.Rows)
                {
                    string[] champs = row.ItemArray.Select(field => field?.ToString() ?? "").ToArray();
                    sb.AppendLine(string.Join(";", champs));
                }

                // Écriture dans le fichier avec l'encodage UTF-8
                File.WriteAllText(save.FileName, sb.ToString(), Encoding.UTF8);
            }

            MessageBox.Show("Export CSV Done.");
        }
    }
}
