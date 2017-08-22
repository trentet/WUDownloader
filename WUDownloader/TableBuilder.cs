using System;
using System.Collections.Generic;
using System.Data;

namespace WUDownloader
{
    static class TableBuilder
    {
        public static DataTable BuildTableSchema(string tableName, string[] headers)
        {
            //Create DataTable, Create DataColumns, and Add DataColumns to DataTable
            DataTable table =  new DataTable(tableName);
            DataColumn[] columns = new DataColumn[headers.Length];
            for (int x = 0; x < headers.Length; x++)
            {
                bool readOnly = false;
                bool isUnique = false;
                if (x == 0)
                {
                    readOnly = true;
                    isUnique = true;
                }

                DataColumn column = CreateColumn("System.String", headers[x], readOnly, isUnique);
                columns[x] = column;
            }

            table = AddColumnsToTable(table, columns);
            return table;
        }

        //public void buildTableSchemaFromDatabase()
        //{
        //    //Create DataSet, Update Catalog DataTable, Create DataColumns, and Add DataColumns to DataTable

        //    table = Dataset.Tables.Add(Configuration.TableName); //Get name of table

        //    List<DataColumn> columns = new List<DataColumn>(new DataColumn[] { columnListGoesHere });
        //    table = addColumnsToTable(columns);
        //    //columnCount = columns.Count();
        //}

        public static DataTable PopulateTableFromCsv(this DataTable table, List<string> csvStringList, bool hasHeaders)
        {
            int startIndex = 0;
            if (hasHeaders == true)
            {
                startIndex = 1; //allows for skipping of headers
            }

            for (int x = startIndex; x < csvStringList.Count; x++)
            {
                Object[] rowContent = csvStringList[x].Replace("\"", "").Split('|'); //Separates out each element in between quotes
                DataRow row = CreateDataRow(table, AssignTypesToData(table, rowContent)); //creates DataRow with data types that match the table schema
                table.Rows.Add(row);
            }
            return table;
        }
        public static DataTable PopulateTableFromCsv(this DataTable table, List<string> csvStringList)
        {
            int startIndex = 1; //skips headers
            for (int x = startIndex; x < csvStringList.Count; x++)
            {
                Object[] rowContent = csvStringList[x].Replace("\"", "").Split('|'); //Separates out each element in between quotes
                DataRow row = CreateDataRow(table, AssignTypesToData(table, rowContent)); //creates DataRow with data types that match the table schema
                table.Rows.Add(row);
            }
            return table;
        }

        private static DataTable AddColumnsToTable(this DataTable table, IEnumerable<DataColumn> columns)
        {
            foreach (DataColumn column in columns)
            {
                table.Columns.Add(column);
            }
            return table;
        }
        private static DataColumn CreateColumn(string columnType, string columnName, Boolean readOnly, Boolean isUnique)
        {
            // Create new DataColumn, set DataType, 
            // ColumnName and add to DataTable.    
            DataColumn column = new DataColumn
            {
                DataType = Type.GetType(columnType),
                ColumnName = columnName,
                ReadOnly = readOnly,
                Unique = isUnique
            };
            return column;
        }
        private static DataTable SetPrimaryKeyColumn(this DataTable table, string primaryColumnName)
        {
            // Make the ID column the primary key column.
            DataColumn[] PrimaryKeyColumns = new DataColumn[1];
            PrimaryKeyColumns[0] = table.Columns[primaryColumnName];
            table.PrimaryKey = PrimaryKeyColumns;
            return table;
        }
        private static DataRow CreateDataRow(DataTable table, Object[] cellData)
        {
            List<string> columnNames = new List<string>();
            foreach (DataColumn column in table.Columns) //Gets list of all column names in the table
            {
                columnNames.Add(column.ColumnName);
            }

            DataRow newRow = table.NewRow();

            for (int x = 0; x < cellData.Length; x++)
            {
                newRow[columnNames[x]] = cellData[x];
            }
            return newRow;
        }
        private static Object[] AssignTypesToData(DataTable table, Object[] data)
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

        public static List<object> GetAllDataFromColumn(DataTable table, string columnName)
        {
            int columnIndex = table.Columns[columnName].Ordinal;
            List<object> listOfDataFromColumn = new List<object>(table.Rows.Count);
            foreach (DataRow row in table.Rows)
            {
                listOfDataFromColumn.Add((object)row[columnIndex]);
            }
            return listOfDataFromColumn;
        }
        public static List<object> GetAllDataFromColumn(DataTable table, int columnIndex)
        {
            List<object> listOfDataFromColumn = new List<object>(table.Rows.Count);
            foreach (DataRow row in table.Rows)
            {
                listOfDataFromColumn.Add((object)row[columnIndex]);
            }
            return listOfDataFromColumn;
        }

        public static Type GetColumnType(DataTable table, string columnName)
        {
            Type columnType = table.Columns[columnName].DataType;
            return columnType;
        }
        public static Type GetColumnType(DataTable table, int ordinalPosition)
        {
            Type columnType = table.Columns[ordinalPosition].DataType;
            return columnType;
        }






        //private static DataSet dataset = new DataSet(Configuration.TableName);
        //private static DataTable table;
        ////private string tableName = "Update Catalog";
        //private int columnCount;

        //public static DataTable Table { get => table; set => table = value; }
        //public static DataSet Dataset { get => dataset; set => dataset = value; }
        ////public string TableName { get => tableName; set => tableName = value; }

        //public void buildTableSchema()
        //{
        //    //Create DataSet, Update Catalog DataTable, Create DataColumns, and Add DataColumns to DataTable

        //    table = Dataset.Tables.Add(Configuration.TableName);
        //    DataColumn id = createColumn("System.String", "id", true, true);
        //    DataColumn title = createColumn("System.String", "title", false, false);
        //    DataColumn product = createColumn("System.String", "product", false, false);
        //    DataColumn classification = createColumn("System.String", "classification", false, false);
        //    DataColumn lastUpdated = createColumn("System.DateTime", "lastUpdated", false, false);
        //    DataColumn version = createColumn("System.String", "version", false, false);
        //    DataColumn size = createColumn("System.String", "size", false, false);
        //    DataColumn downloadUrls = createColumn("System.String", "downloadUrls", false, false);

        //    List<DataColumn> columns = new List<DataColumn>(new DataColumn[] { id, title, product, classification, lastUpdated, version, size, downloadUrls });
        //    table = addColumnsToTable(columns);
        //    columnCount = columns.Count();
        //}
        //public void exportTableSchemaToFile(DataTable table)
        //{
        //    List<string> lines = new List<string>();
        //    foreach (DataColumn column in table.Columns)
        //    {
        //        lines.Add("DataType=" + column.DataType.ToString());
        //        lines.Add("ColumnName=" + column.ColumnName.ToString());
        //        lines.Add("ReadOnly=" + column.ReadOnly.ToString());
        //        lines.Add("Unique=" + column.Unique.ToString());

        //    }
        //}
        //public void populateTableFromCsv(List<string> csv, bool hasHeaders)
        //{
        //    int startIndex = 0;
        //    if (hasHeaders == true)
        //    {
        //        startIndex = 1; //allows for skipping of headers
        //    }

        //    for (int x = startIndex; x < csv.Count; x++)
        //    {
        //        Object[] rowContent = csv[x].Replace("\"","").Split('|'); //Separates out each element in between quotes
        //        DataRow row = createDataRow(assignTypesToData(rowContent)); //creates DataRow with data types that match the table schema
        //        table.Rows.Add(row);
        //    }
        //}

        //public void populateTableFromSite(HtmlDocument siteAsHtml, string tablePath, string fileName)
        //{
        //    List<DataRow> typedDataRows = getTypedDataRowsFromHTML(siteAsHtml); //stores all DataRows from HtmlDocument
        //    foreach (DataRow datarow in typedDataRows) //Adds rows to table
        //    {
        //        AddRowToTable(datarow);
        //        FileIO.ExportDataTableToCSV(table, tablePath, fileName); //Saves table to CSV
        //    }
        //}

        //private DataTable addColumnsToTable(List<DataColumn> columns)
        //{
        //    foreach(DataColumn column in columns)
        //    {
        //        // Add the Column to the DataColumnCollection.
        //        table.Columns.Add(column);
        //    }
        //    return table;
        //}
        //private DataColumn createColumn(string columnType, string columnName, Boolean readOnly, Boolean isUnique)
        //{
        //    // Create new DataColumn, set DataType, 
        //    // ColumnName and add to DataTable.    
        //    DataColumn column = new DataColumn();
        //    column.DataType = System.Type.GetType(columnType);
        //    column.ColumnName = columnName;
        //    column.ReadOnly = readOnly;
        //    column.Unique = isUnique;
        //    return column;
        //}
        //private void setPrimaryKeyColumn(string primaryColumnName)
        //{
        //    // Make the ID column the primary key column.
        //    DataColumn[] PrimaryKeyColumns = new DataColumn[1];
        //    PrimaryKeyColumns[0] = table.Columns[primaryColumnName];
        //    table.PrimaryKey = PrimaryKeyColumns;
        //}
        //private DataRow createDataRow(Object[] cellData)
        //{
        //    List<string> columnNames = new List<string>();
        //    foreach (DataColumn column in table.Columns) //Gets list of all column names in the table
        //    {
        //        columnNames.Add(column.ColumnName);
        //    }

        //    DataRow newRow = dataset.Tables[Configuration.TableName].NewRow();

        //    for (int x = 0; x < cellData.Length; x++)
        //    {
        //        newRow[columnNames[x]] = cellData[x];
        //    }
        //    return newRow;
        //}
        //private void AddRowToTable(DataRow row)
        //{
        //    dataset.Tables[Configuration.TableName].Rows.Add(row);
        //}
        //private Object[] assignTypesToData(Object[] data)
        //{
        //    Object[] convertedDatas = new Object[data.Length];
        //    List<DataColumn> columns = new List<DataColumn>();
        //    int x = 0;
        //    foreach (DataColumn column in table.Columns) // iterates through each column in table
        //    {
        //        //converts data at current index to data type matching current indexes column
        //        convertedDatas[x] = Convert.ChangeType(data[x], column.DataType); 
        //        x++;
        //    }

        //    return convertedDatas;
        //}
        //private List<DataRow> getTypedDataRowsFromHTML(HtmlDocument siteAsHtml)
        //{
        //    //All elements with tag of "td"
        //    HtmlElementCollection cellElements = siteAsHtml.GetElementsByTagName("td");
        //    List<string> unparsedRow = new List<string>();
        //    foreach (HtmlElement elem in cellElements)
        //    {
        //        if (elem.GetAttribute("className").Contains("resultsbottomBorder") && !elem.GetAttribute("className").Contains("resultsIconWidth")) //excludes empty column
        //        {
        //            string line = elem.InnerHtml;
        //            unparsedRow.Add(line); 
        //        }
        //    }
        //    Object[] cellData = new Object[columnCount];
        //    int numberOfColumnsNotParsedFromSite = 1;
        //    int numOfRows = unparsedRow.Count / (columnCount - numberOfColumnsNotParsedFromSite);
        //    Parser p = new Parser();
        //    List<DataRow> datarows = new List<DataRow>();
        //    for (int x = 0; x < numOfRows; x++)
        //    {
        //        cellData = Parser.parseHtmlRow(cellData.Length, unparsedRow, x);
        //        DataRow datarow = createDataRow(assignTypesToData(cellData));
        //        datarows.Add(datarow);
        //    }
        //    return datarows;
        //}
        //public List<object> getAllDataFromColumn(string columnName)
        //{
        //    int columnIndex = table.Columns[columnName].Ordinal;
        //    List<object> listOfDataFromColumn = new List<object>(table.Rows.Count);
        //    foreach (DataRow row in table.Rows)
        //    {
        //        listOfDataFromColumn.Add((object)row[columnIndex]);
        //    }
        //    return listOfDataFromColumn;
        //}
        //public Type getColumnType(string columnName)
        //{
        //    Type columnType = table.Columns[columnName].DataType;
        //    return columnType;
        //}
    }
}
