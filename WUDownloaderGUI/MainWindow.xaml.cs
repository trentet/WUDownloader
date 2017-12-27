using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TableBuilderLibrary;
using WUDownloader;

namespace WUDownloaderGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Import.Content = "Import";
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files (*.csv)|*.csv";
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName.Replace(".csv", "");
                List<string> csvStringList = FileIO.ImportCsvToStringList(filePath);
                bool hasHeaders = true;

                //Get Headers 
                string[] headers = Parser.ParseHeadersFromCsvStringList(csvStringList);

                DataTable table = TableBuilder.BuildTableSchema("Table", headers);
                char delimiter = '|';

                //Populate Table
                table.PopulateTableFromCsv(filePath, delimiter, hasHeaders);

                grid.ItemsSource = table.AsDataView();
            }
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            DataTable table = new DataTable();
            table = ((DataView)grid.ItemsSource).ToTable();

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
            saveFileDialog.RestoreDirectory = true;
            if (saveFileDialog.ShowDialog() == true)
            {
                if (saveFileDialog.FileName != "")
                {
                    FileIO.ExportDataTableToCSV(table, saveFileDialog.FileName);
                }
            }

            //using (SqlConnection conn = new SqlConnection())
            //{
            //    string destinationTableName = "test";
            //    string[] restrictions = new string[4];
            //    restrictions[2] = destinationTableName;
            //    DataTable t = conn.GetSchema("Columns", restrictions);


            //    conn.ConnectionString = "Server=[server_name];Database=[database_name];Trusted_Connection=true";
            //    using (var bulkCopy = new SqlBulkCopy(conn.ConnectionString, SqlBulkCopyOptions.KeepIdentity))
            //    {
            //        // my DataTable column names match my SQL Column names, so I simply made this loop. However if your column names don't match, just pass in which datatable name matches the SQL column name in Column Mappings
            //        foreach (DataColumn col in table.Columns)
            //        {
            //            //Add(sourceColumnName, destinationColumnName)
            //            bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
            //        }

            //        bulkCopy.BulkCopyTimeout = 600;
            //        bulkCopy.DestinationTableName = destinationTableName;
            //        bulkCopy.WriteToServer(table);
            //    }
            //}
        }
    }
}
