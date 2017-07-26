using System.Collections.Generic;
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

        public static List<string>[] getDownloadUrlsFromTable(DataTable table, string title, List<string> osList)
        {
            string tableName = table.TableName;
            string orPieces = "";
            
            for(int x = 0; x < osList.Count; x++)
            {
                if (x == osList.Count - 1)
                {
                    orPieces += "os Like '%" + osList[x] + "%'";
                }
                else
                {
                    orPieces += "os Like '%" + osList[x] + "%' or ";
                }
            }

            // Presuming the DataTable has a column named Date.
            string expression = "title Like '%" + title + "%' and (" + orPieces + ")";
            string sortOrder = "os, lastUpdated DESC";

            // Use the Select method to find all rows matching the filter.
            DataRow[] foundRows = table.Select(expression, sortOrder);
            List<DataRow> prunedRows = new List<DataRow>();

            List<string> osGrabbed = new List<string>();
            for (int x = 0; x < foundRows.Length; x++)
            {
                string currentRowOS = foundRows[x].ItemArray[2].ToString();
                if (!osGrabbed.Contains(currentRowOS))
                {
                    osGrabbed.Add(currentRowOS);
                    prunedRows.Add(foundRows[x]);
                }
            }

            //First row in foundRows, at the 7th column (downloadUrls)
            List<string> oses = new List<string>();
            List<string> downloadUrls = new List<string>();
            foreach (DataRow row in prunedRows)
            {
                oses.Add(row.ItemArray[2].ToString());
                downloadUrls.Add(row.ItemArray[7].ToString());
            }

            List<string>[] oses_and_downloadUrls = new List<string>[] {oses, downloadUrls };

            return oses_and_downloadUrls;
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
