using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WUDownloader
{
    class TableBuilder
    {
        private DataSet dataset = new DataSet("UpdateCatalog");
        private DataTable table;
        private string tableName = "Update Catalog";
        private FileIO f = new FileIO();

        public DataTable Table { get => table; set => table = value; }
        public DataSet Dataset { get => dataset; set => dataset = value; }
        public string TableName { get => tableName; set => tableName = value; }

        public void buildTable(string tablePath)
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
            //DataColumn buttonHTML = createColumn("System.String", "buttonHTML", false, false);

            List<DataColumn> columns = new List<DataColumn>(new DataColumn[] { id, title, os, classification, lastUpdated, version, size });//, buttonHTML });
            table = addColumnsToTable(columns);

            f.ExportDataTableToCSV(table, tablePath);
        }
        public void populateTable(HtmlDocument siteAsHtml, string tablePath)
        {
            List<DataRow> typedDataRows = getTypedDataRowsFromHTML(siteAsHtml);
            foreach (DataRow datarow in typedDataRows) //Adds rows to table
            {
                AddRowToTable(datarow);
                f.ExportDataTableToCSV(table, tablePath);
            }
        }
        public DataTable addColumnsToTable(List<DataColumn> columns)
        {
            foreach(DataColumn column in columns)
            {
                // Add the Column to the DataColumnCollection.
                table.Columns.Add(column);
            }
            return table;
        }
        public DataColumn createColumn(string columnType, string columnName, Boolean readOnly, Boolean isUnique)
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
        public DataTable setPrimaryKeyColumn(string primaryColumnName)
        {
            // Make the ID column the primary key column.
            DataColumn[] PrimaryKeyColumns = new DataColumn[1];
            PrimaryKeyColumns[0] = table.Columns[primaryColumnName];
            table.PrimaryKey = PrimaryKeyColumns;
            return table;
        }
        public DataRow createDataRow(Object[] cellData)
        {
            List<string> columnNames = new List<string>();
            foreach (DataColumn column in table.Columns)
            {
                columnNames.Add(column.ColumnName);
            }
            DataRow newRow = dataset.Tables[tableName].NewRow();

            for (int x = 0; x < cellData.Length; x++)
            {
                Type columntype = cellData[x].GetType();
                var data = cellData[x];
                newRow[columnNames[x]] = data;
            }
            return newRow;
        }
        public void AddRowToTable(DataRow row)
        {
            dataset.Tables[tableName].Rows.Add(row);
        }
        public Object[] assignTypesToData(Object[] data)
        {
            Object[] convertedDatas = new Object[data.Length];
            List<DataColumn> columns = new List<DataColumn>();
            int x = 0;
            foreach (DataColumn column in table.Columns)
            {
                convertedDatas[x] = Convert.ChangeType(data[x], column.DataType);
                x++;
            }

            return convertedDatas;
        }
        public List<DataRow> getTypedDataRowsFromHTML(HtmlDocument siteAsHtml)
        {
            HtmlElementCollection cellElements = siteAsHtml.GetElementsByTagName("td");
            List<DataRow> datarows = new List<DataRow>();
            Parser p = new Parser();
            List<string> unparsedRow = new List<string>();
            foreach (HtmlElement elem in cellElements)
            {
                if (elem.GetAttribute("className").Contains("resultsbottomBorder") && !elem.GetAttribute("className").Contains("resultsIconWidth"))
                {
                    string line = elem.InnerHtml;
                    unparsedRow.Add(line);
                }
            }
            Object[] cellData = new Object[7];
            int numOfRows = unparsedRow.Count / 7;
            for (int x = 0; x < numOfRows; x++)
            {
                cellData = p.parseHtmlRow(cellData.Length, unparsedRow, x);
                DataRow datarow = createDataRow(assignTypesToData(cellData));
                datarows.Add(datarow);
            }
            return datarows;
        }
    }
}
