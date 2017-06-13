using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WUDownloader
{
    class DataTables
    {
        public DataTable addColumnsToTable(DataTable table, List<DataColumn> columns)
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
        public DataTable setPrimaryKeyColumn(DataTable table, string primaryColumnName)
        {
            // Make the ID column the primary key column.
            DataColumn[] PrimaryKeyColumns = new DataColumn[1];
            PrimaryKeyColumns[0] = table.Columns[primaryColumnName];
            table.PrimaryKey = PrimaryKeyColumns;
            return table;
        }
        public DataRow createDataRow(DataSet dataSet, DataTable table, string tableName, Object[] columnData)
        {
            List<string> columnNames = new List<string>();
            foreach (DataColumn column in table.Columns)
            {
                columnNames.Add(column.ColumnName);
            }
            DataRow newRow = dataSet.Tables[tableName].NewRow();

            for (int x = 0; x < columnData.Length; x++)
            {
                Type columntype = columnData[x].GetType();
                var data = columnData[x];
                newRow[columnNames[x]] = data;
            }
            return newRow;
        }
        public DataSet AddRowToTable(DataSet dataSet, string tableName, DataRow row)
        {
            dataSet.Tables[tableName].Rows.Add(row);
            return dataSet;
        }
    }
}
