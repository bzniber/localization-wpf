using Microsoft.VisualBasic;
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
            dt.Columns.Add("es");

            dt.TableName = "DataTable";
            datagrid.ItemsSource = dt.DefaultView;
        }

        void UpdateDataGrid()
        {
            datagrid.ItemsSource = null;
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


        private void OnImportClickedJSON(object sender, RoutedEventArgs e)
        {

        }

        private void OnImportClickedCSV(object sender, RoutedEventArgs e)
        {
            InportCSV();
        }

        private void OnImportClickedXML(object sender, RoutedEventArgs e)
        {
            ImportXML();
        }

        private void OnAddColumnClicked(object sender, RoutedEventArgs e)
        {
            string userInput = Interaction.InputBox("Enter column name:", "Input Required", "");

            if (!string.IsNullOrEmpty(userInput))
            {
                dt.Columns.Add(userInput);
                UpdateDataGrid();
            }
        }

        private void OnAddRowClicked(object sender, RoutedEventArgs e)
        {
            dt.Rows.Add();
            UpdateDataGrid();
        }

        private void OnRemoveColumnClicked(object sender, RoutedEventArgs e)
        {
            int count = dt.Columns.Count;
            if (count > 0)
            {
                dt.Columns.RemoveAt(count - 1);
                UpdateDataGrid();
            }
        }

        private void OnRemoveRowClicked(object sender, RoutedEventArgs e)
        {
            int count = dt.Rows.Count;
            if (count > 0)
            {
                dt.Rows.RemoveAt(count-1);
                UpdateDataGrid();
            }
        }
    }
}