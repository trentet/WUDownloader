using System.Collections.Generic;
using System.Data;
using System.Text;

namespace WUDownloader
{
    class QueryController
    {
        public static string getIDFromTable(DataTable table, string title, string product)
        {
            string tableName = table.TableName;

            // Presuming the DataTable has a column named Date.
            string expression = "title Like '%" + EscapeLikeValue(title) + "%' and product Like '%" + EscapeLikeValue(product) + "%'";
            string sortOrder = "lastUpdated DESC";
            
            // Use the Select method to find all rows matching the filter.
            DataRow[] foundRows = table.Select(expression, sortOrder);
            
            //First row in foundRows, at the 1st column (id)
            string id = foundRows[0].ItemArray[0].ToString();

            return id;
        }


        public static List<string>[] getDownloadUrlsFromTable(DataTable table, string title, List<string> productList)
        {
            string tableName = table.TableName;
            string orPieces = "";
            
            for(int x = 0; x < productList.Count; x++)
            {
                if (x == productList.Count - 1)
                {
                    orPieces += "product Like '%" + EscapeLikeValue(productList[x]) + "%'";
                }
                else
                {
                    orPieces += "product Like '%" + EscapeLikeValue(productList[x]) + "%' or ";
                }
            }

            // Presuming the DataTable has a column named Date.
            string expression = "title Like '%" + EscapeLikeValue(title) + "%' and (" + orPieces + ")";
            string sortOrder = "product, lastUpdated DESC";

            // Use the Select method to find all rows matching the filter.
            DataRow[] foundRows = table.Select(expression, sortOrder);
            List<DataRow> prunedRows = new List<DataRow>();

            List<string> productGrabbed = new List<string>();
            int columnIndex = table.Columns["product"].Ordinal;
            for (int x = 0; x < foundRows.Length; x++)
            {
                string currentRowProduct = foundRows[x].ItemArray[columnIndex].ToString();
                if (!productGrabbed.Contains(currentRowProduct))
                {
                    productGrabbed.Add(currentRowProduct);
                    prunedRows.Add(foundRows[x]);
                }
            }

            //First row in foundRows, at the 7th column (downloadUrls)
            List<string> products = new List<string>();
            List<string> downloadUrls = new List<string>();
            foreach (DataRow row in prunedRows)
            {
                products.Add(row.ItemArray[2].ToString());
                downloadUrls.Add(row.ItemArray[7].ToString());
            }

            List<string>[] products_and_downloadUrls = new List<string>[] { products, downloadUrls };

            return products_and_downloadUrls;
        }

        public static bool doesUpdateTitleExistInTable(DataTable table, string title)
        {
            bool exists = false;

            string tableName = table.TableName;

            // Presuming the DataTable has a column named Date.
            string expression = "title LIKE '%" + EscapeLikeValue(title) + "%'";
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
        private static string EscapeLikeValue(string value)
        {
            StringBuilder sb = new StringBuilder(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                char c = value[i];
                switch (c)
                {
                    case ']':
                    case '[':
                    case '%':
                    case '*':
                        sb.Append("[").Append(c).Append("]");
                        break;
                    case '\'':
                        sb.Append("''");
                        break;
                    default:
                        sb.Append(c);
                        break;
                }
            }
            return sb.ToString();
        }
    }
}
