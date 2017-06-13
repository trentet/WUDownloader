using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Linq;

namespace WUDownloader
{
    class Controller
    {
        FileIO f = new FileIO();
        Parser p = new Parser();
        DataTables d = new DataTables();
        DataSet dataset = new DataSet("UpdateCatalog");
        string CATALOG_URL = "https://www.catalog.update.microsoft.com/Search.aspx?q=";
        string OS = "Windows Server 2012 R2";
        public void Run()
        {
            List<string> lines = f.ImportFileToArray(@"D:\Input\Updates.txt");
            List<string> updateNames = p.ParseLinesContaining(lines, "(KB");

            foreach (string name in updateNames) //For each update
            {
                string kb = name.Split('(', ')')[1];
                openURLinNewTab(kb);
                //doStuff(name, kb);
            }
            Console.WriteLine("Exiting...");
            System.Console.ReadKey();
        }

        public void openURLinNewTab(string kb)
        {
            Process.Start(CATALOG_URL + kb);
        }

        public void doStuff(string name, string kb)
        {
            DataTable updateCatalogTable = buildTable();
            string tableName = updateCatalogTable.TableName;
            List<Object[]> dataFromRows = getDataFromRowsForUpdate(name, kb, updateCatalogTable);

            foreach (Object[] objArray in dataFromRows) //Adds data from website to table as rows
            {
                DataRow row = d.createDataRow(dataset, updateCatalogTable, tableName, objArray);
                this.dataset = d.AddRowToTable(dataset, tableName, row);
            }
            DataRow[] foundRows = GetRowsByFilter(tableName, name, OS);

            var buttonID = foundRows[0].ItemArray[0].ToString();
            HtmlDocument siteAsHtml = getUpdateCatalogHTML(name, kb);
            Console.WriteLine("Attempting to click button: ");
            ClickButton(siteAsHtml, buttonID);
        }

        public void ClickButton(HtmlDocument siteAsHtml, string buttonID)
        {
            var button = siteAsHtml.GetElementsByTagName("button")
                     .Cast<HtmlElement>()
                     .FirstOrDefault(m => m.GetAttribute("input") == buttonID);
            if (button != null)
                button.InvokeMember("click");
        }

        public DataTable buildTable()
        {
            //Create DataSet, Update Catalog DataTable, Create DataColumns, and Add DataColumns to DataTable
            string tableName = "Update Catalog";
            DataTable updateCatalogTable = dataset.Tables.Add(tableName);
            DataColumn id = d.createColumn("System.String", "id", true, true);
            DataColumn title = d.createColumn("System.String", "title", false, false);
            DataColumn os = d.createColumn("System.String", "os", false, false);
            DataColumn classification = d.createColumn("System.String", "classification", false, false);
            DataColumn lastUpdated = d.createColumn("System.DateTime", "lastUpdated", false, false);
            DataColumn version = d.createColumn("System.String", "version", false, false);
            DataColumn size = d.createColumn("System.String", "size", false, false);
            DataColumn buttonHTML = d.createColumn("System.String", "buttonHTML", false, false);

            List<DataColumn> columns = new List<DataColumn>(new DataColumn[] { id, title, os, classification, lastUpdated, version, size, buttonHTML });
            updateCatalogTable = d.addColumnsToTable(updateCatalogTable, columns);

            return updateCatalogTable;
        }

        private DataRow[] GetRowsByFilter(string tableName, string title, string os)
        {
            DataTable table = dataset.Tables[tableName];

            // Presuming the DataTable has a column named Date.
            string expression = "title  = '" + title + "' and os = '" + os + "'";
            string sortOrder = "lastUpdated DESC";
            DataRow[] foundRows;

            // Use the Select method to find all rows matching the filter.
            foundRows = table.Select(expression, sortOrder);

            return foundRows;
            // Print column 0 of each returned row.
            //for (int i = 0; i < foundRows.Length; i++)
            //{
            //    Console.WriteLine(foundRows[i][0]);
            //}
        }

        public List<Object[]> getDataFromRowsForUpdate(string name, string kb, DataTable table)
        {
            HtmlDocument siteAsHTML = getUpdateCatalogHTML(name, kb);
            HtmlElementCollection siteTableHTML = siteAsHTML.GetElementsByTagName("td");
            List<List<string>> dataStringsFromRows = getDataStringsFromRowsInCatalog(siteTableHTML);
            List<Object[]> dataFromRows = new List<Object[]>();
            foreach (List<string> rowData in dataStringsFromRows)
            {
                dataFromRows.Add(assignTypesToData(rowData, table));
            }
            return dataFromRows;
        }

        public HtmlDocument getUpdateCatalogHTML(string name, string kb)
        {
            HtmlDocument siteAsHTML = GetHtmlDocument("");
            Console.WriteLine("Attempting to collect HTML for update: " + name);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(CATALOG_URL + kb);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string htmlString;
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }

                htmlString = readStream.ReadToEnd();
                if (htmlString.Length > 0)
                {
                    siteAsHTML = GetHtmlDocument(htmlString);
                    Console.WriteLine("HTML collected for update: " + name);
                }
                else if (htmlString.Length == 0)
                {
                    Console.WriteLine("HTML not collected for update: " + name);
                }
                
                response.Close();
                readStream.Close();
            }
            else
            {
                Console.WriteLine("HTTPStatusCode Not OK -- HTML not collected for update: " + name);
            }
            return siteAsHTML;
        }

        public List<List<string>> getDataStringsFromRowsInCatalog(HtmlElementCollection elements)
        {
            List<List<string>> dataFromRows = new List<List<string>>();
            Parser p = new Parser();
            List<string> rowHTML = new List<string>();
            foreach (HtmlElement elem in elements)
            {
                if (elem.GetAttribute("className").Contains("resultsbottomBorder") && !elem.GetAttribute("className").Contains("resultsIconWidth"))
                {
                    string line = elem.InnerHtml;
                    rowHTML.Add(line);
                }
            }
            List<string> rowData = new List<string>();
            int numOfRows = rowHTML.Count / 7;
            for (int x = 0; x < numOfRows; x++)
            {
                rowData = p.getRowDataFromHTML(rowHTML, x);
                dataFromRows.Add(rowData);
            }
            
            return dataFromRows;
        }

        public Object[] assignTypesToData(List<string> data, DataTable table)
        {
            Object[] convertedDatas = new Object[data.Count];
            List<DataColumn> columns = new List<DataColumn>();
            int x = 0;
            foreach (DataColumn column in table.Columns)
            {
                convertedDatas[x] = Convert.ChangeType(data[x], column.DataType);
                x++;
            }

            return convertedDatas;
        }

        public System.Windows.Forms.HtmlDocument GetHtmlDocument(string html)
        {
            WebBrowser browser = new WebBrowser();
            browser.ScriptErrorsSuppressed = true; //not necessesory you can remove it
            browser.DocumentText = html;
            browser.Document.OpenNew(true);
            browser.Document.Write(html);
            browser.Refresh();
            return browser.Document;
        }
    }
}
