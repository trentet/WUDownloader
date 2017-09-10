using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TableBuilderLibrary;

namespace WUDownloader
{
    public static class MyExtensions
    {
        public static DataTable PopulateTableFromSite(this DataTable table, HtmlDocument siteAsHtml, string tablePath, string fileName)
        {
            List<DataRow> typedDataRows = Parser.GetTypedDataRowsFromHTML(table, siteAsHtml); //stores all DataRows from HtmlDocument
            foreach (DataRow datarow in typedDataRows) //Adds rows to table
            {
                table.AddRowToTable(datarow);
                FileIO.ExportDataTableToCSV(table, tablePath, fileName); //Saves table to CSV
            }
            return table;
        }
    }
}
