using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace WUDownloader
{
    class FileIO
    {
        public static List<string> ImportFileToStringList(string filepath)
        {
            List<string> lines = System.IO.File.ReadAllLines(filepath).ToList();
            return lines;
        }

        public static void ExportStringArrayToFile(string filepath, string[] lines)
        {
            System.IO.File.WriteAllLines(filepath, lines);
        }

        public static List<string> ImportCsvToStringList(string filepath)
        {
            List<string> csv = System.IO.File.ReadAllLines(filepath).ToList();
            return csv;
        }
        public static void ExportDataTableToCSV(DataTable table, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath + ".csv"))
            {
                WriteDataTable(table, writer, true);
                writer.Close();
            }
        }

        private static void WriteDataTable(DataTable sourceTable, TextWriter writer, bool includeHeaders)
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
