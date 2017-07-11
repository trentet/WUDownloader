using System.Data;

namespace WUDownloader
{
    class QueryController
    {
        public static string getIDFromTable(DataTable table, string title, string os)
        {
            string tableName = table.TableName;

            // Presuming the DataTable has a column named Date.
            string expression = "title Like '%" + title + "%' and os Like '%" + os + "%'";
            string sortOrder = "lastUpdated DESC";
            
            // Use the Select method to find all rows matching the filter.
            DataRow[] foundRows = table.Select(expression, sortOrder);

            //First row in foundRows, at the 1st column (id)
            string id = foundRows[0].ItemArray[0].ToString();

            return id;
        }

        public static string getDownloadUrlsFromTable(DataTable table, string title, string os)
        {
            string tableName = table.TableName;

            // Presuming the DataTable has a column named Date.
            string expression = "title Like '%" + title + "%' and os Like '%" + os + "%'";
            string sortOrder = "lastUpdated DESC";

            // Use the Select method to find all rows matching the filter.
            DataRow[] foundRows = table.Select(expression, sortOrder);

            //First row in foundRows, at the 7th column (downloadUrls)
            string downloadUrls = foundRows[0].ItemArray[7].ToString();

            return downloadUrls;
        }

        public static bool doesUpdateTitleExistInTable(DataTable table, string title)
        {
            bool exists = false;

            string tableName = table.TableName;

            // Presuming the DataTable has a column named Date.
            string expression = "title LIKE '%" + title + "%'";
            string sortOrder = "lastUpdated DESC";

            // Use the Select method to find all rows matching the filter.
            DataRow[] foundRows = table.Select(expression, sortOrder);

            //If foundRows.Length is 0, then the query returned nothing
            if (foundRows.Length > 0)
            {
                exists = true;
            }

            return exists;
        }
    }
}
