using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace WUDownloader
{
    class TableBuilder
    {
        private DataSet dataset = new DataSet("UpdateCatalog");
        private DataTable table;
        private string tableName = "Update Catalog";
        private int columnCount;

        public DataTable Table { get => table; set => table = value; }
        public DataSet Dataset { get => dataset; set => dataset = value; }
        public string TableName { get => tableName; set => tableName = value; }

        public void buildTableSchema(string tablePath)
        {
            //Create DataSet, Update Catalog DataTable, Create DataColumns, and Add DataColumns to DataTable

            table = Dataset.Tables.Add(tableName);
            DataColumn id = createColumn("System.String", "id", true, true);
            DataColumn title = createColumn("System.String", "title", false, false);
            DataColumn os = createColumn("System.String", "os", false, false);
            DataColumn classification = createColumn("System.String", "classification", false, false);
            DataColumn lastUpdated = createColumn("System.DateTime", "lastUpdated", false, false);
            DataColumn version = createColumn("System.String", "version", false, false);
            DataColumn size = createColumn("System.String", "size", false, false);
            DataColumn downloadUrls = createColumn("System.String", "downloadUrls", false, false);

            List<DataColumn> columns = new List<DataColumn>(new DataColumn[] { id, title, os, classification, lastUpdated, version, size, downloadUrls });
            table = addColumnsToTable(columns);
            columnCount = columns.Count();
        }
        public void populateTableFromCsv(List<string> csv, bool hasHeaders)
        {
            int startIndex = 0;
            if (hasHeaders == true)
            {
                startIndex = 1; //allows for skipping of headers
            }

            for (int x = startIndex; x < csv.Count; x++)
            {
                Object[] rowContent = csv[x].Replace("\",\"", "\"|\"").Replace("\"","").Split('|'); //Separates out each element in between quotes
                DataRow row = createDataRow(assignTypesToData(rowContent)); //creates DataRow with data types that match the table schema
                table.Rows.Add(row);
            }
        }

        public void populateTableFromSite(HtmlDocument siteAsHtml, string tablePath)
        {
            List<DataRow> typedDataRows = getTypedDataRowsFromHTML(siteAsHtml); //stores all DataRows from HtmlDocument
            foreach (DataRow datarow in typedDataRows) //Adds rows to table
            {
                AddRowToTable(datarow);
                FileIO.ExportDataTableToCSV(table, tablePath); //Saves table to CSV
            }
        }

        private DataTable addColumnsToTable(List<DataColumn> columns)
        {
            foreach(DataColumn column in columns)
            {
                // Add the Column to the DataColumnCollection.
                table.Columns.Add(column);
            }
            return table;
        }
        private DataColumn createColumn(string columnType, string columnName, Boolean readOnly, Boolean isUnique)
        {
            // Create new DataColumn, set DataType, 
            // ColumnName and add to DataTable.    
            DataColumn column = new DataColumn();
            column.DataType = System.Type.GetType(columnType);
            column.ColumnName = columnName;
            column.ReadOnly = readOnly;
            column.Unique = isUnique;
            return column;
        }
        private void setPrimaryKeyColumn(string primaryColumnName)
        {
            // Make the ID column the primary key column.
            DataColumn[] PrimaryKeyColumns = new DataColumn[1];
            PrimaryKeyColumns[0] = table.Columns[primaryColumnName];
            table.PrimaryKey = PrimaryKeyColumns;
        }
        private DataRow createDataRow(Object[] cellData)
        {
            List<string> columnNames = new List<string>();
            foreach (DataColumn column in table.Columns) //Gets list of all column names in the table
            {
                columnNames.Add(column.ColumnName);
            }

            DataRow newRow = dataset.Tables[tableName].NewRow();

            for (int x = 0; x < cellData.Length; x++)
            {
                //Type columntype = cellData[x].GetType(); //gets
                //var data = cellData[x];
                newRow[columnNames[x]] = cellData[x];
            }
            return newRow;
        }
        private void AddRowToTable(DataRow row)
        {
            dataset.Tables[tableName].Rows.Add(row);
        }
        private Object[] assignTypesToData(Object[] data)
        {
            Object[] convertedDatas = new Object[data.Length];
            List<DataColumn> columns = new List<DataColumn>();
            int x = 0;
            foreach (DataColumn column in table.Columns) // iterates through each column in table
            {
                //converts data at current index to data type matching current indexes column
                convertedDatas[x] = Convert.ChangeType(data[x], column.DataType); 
                x++;
            }

            return convertedDatas;
        }
        private List<DataRow> getTypedDataRowsFromHTML(HtmlDocument siteAsHtml)
        {
            //All elements with tag of "td"
            HtmlElementCollection cellElements = siteAsHtml.GetElementsByTagName("td");
            List<DataRow> datarows = new List<DataRow>();
            List<string> unparsedRow = new List<string>();
            foreach (HtmlElement elem in cellElements)
            {
                if (elem.GetAttribute("className").Contains("resultsbottomBorder") && !elem.GetAttribute("className").Contains("resultsIconWidth")) //excludes empty column
                {
                    string line = elem.InnerHtml;
                    unparsedRow.Add(line); 
                }
            }
            Object[] cellData = new Object[columnCount];
            int numberOfColumnsNotParsedFromSite = 1;
            int numOfRows = unparsedRow.Count / (columnCount - numberOfColumnsNotParsedFromSite);
            Parser p = new Parser();
            for (int x = 0; x < numOfRows; x++)
            {
                cellData = Parser.parseHtmlRow(cellData.Length, unparsedRow, x);
                DataRow datarow = createDataRow(assignTypesToData(cellData));
                datarows.Add(datarow);
            }
            return datarows;
        }
    }
}
