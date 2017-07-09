using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WUDownloader
{
    class FileIO
    {
        public List<string> ImportFileToArray(string filepath)
        {
            List<string> lines = System.IO.File.ReadAllLines(filepath).ToList();
            return lines;
        }

        public List<string> ImportCsvToStringList(string filepath)//, bool hasHeaders)
        {
            //string csv;
            //if (hasHeaders == true)
            //{
                List<string> csv = System.IO.File.ReadAllLines(filepath).ToList();

                //csvWithHeaders.RemoveAt(0);
                //csv = string.Join("", csvWithHeaders.ToArray());
            //}
            //else //has no headers
            //{
                //csv = System.IO.File.ReadAllLines(filepath).ToString();
            //}
            
            return csv;
        }
        public void ExportDataTableToCSV(DataTable table, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath + ".csv"))
            {
                WriteDataTable(table, writer, true);
                writer.Close();
            }
        }

        public static void WriteDataTable(DataTable sourceTable, TextWriter writer, bool includeHeaders)
        {
            if (includeHeaders)
            {
                IEnumerable<String> headerValues = sourceTable.Columns
                    .OfType<DataColumn>()
                    .Select(column => QuoteValue(column.ColumnName));

                writer.WriteLine(String.Join(",", headerValues));
            }

            IEnumerable<String> items = null;

            foreach (DataRow row in sourceTable.Rows)
            {
                items = row.ItemArray.Select(o => QuoteValue(o.ToString()));
                writer.WriteLine(String.Join(",", items));
            }

            writer.Flush();
        }

        private static string QuoteValue(string value)
        {
            return String.Concat("\"",
            value.Replace("\"", "\"\""), "\"");
        }
    }
}
