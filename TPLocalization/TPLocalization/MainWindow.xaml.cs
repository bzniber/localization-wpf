using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TPLocalization
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataTable dt = new DataTable();

        public MainWindow()
        {
            InitializeComponent();

            dt.Columns.Add("id");
            dt.Columns.Add("en");
            dt.Columns.Add("fr");
            dt.Columns.Add("comment");

            string[] texts = { "play", "play", "jouer", "bouton jouer" };
            //ajout de lignes
            dt.Rows.Add(texts);
            dt.TableName = "DataTable";
            datagrid.ItemsSource = dt.DefaultView;
        }

        private void OnExportClickedJSON(object sender, RoutedEventArgs e)
        {
            ExportJSON();
        }

        private void OnExportClickedCSV(object sender, RoutedEventArgs e)
        {
            ExportCSV();
        }

        private void OnExportClickedXML(object sender, RoutedEventArgs e)
        {
            ExportXML();
        }

        private void OnExportClickedCSharp(object sender, RoutedEventArgs e)
        {
            ExportCSharp();
        }
        private void OnExportClickedPlusPlus(object sender, RoutedEventArgs e)
        {
            ExportCpp();
        }
    }
}