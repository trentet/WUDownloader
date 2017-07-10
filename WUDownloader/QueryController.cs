using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WUDownloader
{
    class QueryController
    {
        public string getIDFromTable(DataTable table, string title, string os)
        {
            string tableName = table.TableName;

            // Presuming the DataTable has a column named Date.
            string expression = "title Like '%" + title + "%' and os Like '%" + os + "%'";
            string sortOrder = "lastUpdated DESC";
            
            // Use the Select method to find all rows matching the filter.
            DataRow[] foundRows = table.Select(expression, sortOrder);

            string id = foundRows[0].ItemArray[0].ToString();

            return id;
        }

        public string getDownloadUrlsFromTable(DataTable table, string title, string os)
        {
            string tableName = table.TableName;

            // Presuming the DataTable has a column named Date.
            string expression = "title Like '%" + title + "%' and os Like '%" + os + "%'";
            string sortOrder = "lastUpdated DESC";

            // Use the Select method to find all rows matching the filter.
            DataRow[] foundRows = table.Select(expression, sortOrder);

            //DataRow[] foundRows = GetRowsByFilter(dataset, tableName, title, os);

            string downloadUrls = foundRows[0].ItemArray[7].ToString();

            return downloadUrls;
        }

        public bool doesUpdateTitleExistInTable(DataTable table, string title)
        {
            bool exists = false;

            string tableName = table.TableName;

            // Presuming the DataTable has a column named Date.
            string expression = "title LIKE '%" + title + "%'";
            string sortOrder = "lastUpdated DESC";

            // Use the Select method to find all rows matching the filter.
            DataRow[] foundRows = table.Select(expression, sortOrder);

            if (foundRows.Length > 0)
            {
                exists = true;
            }

            return exists;
        }
    }
}
