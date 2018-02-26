using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using TableBuilderLibrary;

namespace WUDownloader
{
    public class FileIO
    {
        public static List<string> ImportFileToStringList(string filepath)
        {
            List<string> lines = System.IO.File.ReadAllLines(filepath).ToList();
            return lines;
        }

        public static void ExportStringListToFile(string filepath, List<string> lines)
        {
            System.IO.File.WriteAllLines(filepath, lines);
        }

        public static List<string> ImportCsvToStringList(string filepath)
        {
            List<string> csv = System.IO.File.ReadAllLines(filepath + ".csv").ToList();
            return csv;
        }

        public static DataTable ImportTableFromCsv()
        {
            bool needsGuid = false;
            //Build table with schema
            DataTable table = TableBuilder.BuildTableSchema(Configuration.TableName, Configuration.TableHeaders, Configuration.TableColumnTypes, needsGuid);
            //Check if file exists
            if (File.Exists(Configuration.TableFolderPath + "\\" + Configuration.TableName + ".csv")) //If exists
            {
                Console.WriteLine("CSV file exists. Importing...");

                //Populate table from file
                table.PopulateTableFromCsv(Configuration.TableFolderPath, Configuration.TableName, ',', true, needsGuid);
                return table;
            }
            else //If not exists
            {
                Console.WriteLine("CSV file does not exists. Generating...");

                ExportDataTableToCSV(table, Configuration.TableFolderPath, Configuration.TableName);
                Console.WriteLine("CSV file saved.");
                return table;
            }
        }

        public static void ExportDataTableToCSV(DataTable table, string folderPath, string fileName)
        {
            using (StreamWriter writer = new StreamWriter(folderPath.TrimEnd('\\') + "\\" + fileName + ".csv"))
            {
                WriteDataTable(table, writer, true);
                writer.Close();
            }
        }

        public static void ExportDataTableToCSV(DataTable table, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
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
