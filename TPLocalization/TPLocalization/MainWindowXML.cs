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
            dt.ReadXml("DataTable.xml");
        }

        public void ExportXML()
        {
            dt.WriteXml("DataTable.xml");
        }
    }
}
