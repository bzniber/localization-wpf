using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows;
using System.Xml.Linq;

namespace TPLocalization
{
    public partial class MainWindow
    {
        public void ImportXML()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            if (ofd != null && ofd.FileName.Length > 1)
            {
                dt.ReadXml(ofd.FileName);
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
