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
            dt.ReadXml(ofd.FileName);
        }

        public void ExportXML()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.ShowDialog();
            dt.WriteXml(sfd.FileName);
        }
    }
}
