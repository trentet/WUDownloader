using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.Text;
using System.Data;

namespace WUDownloader
{
    class Controller
    {
        FileIO f = new FileIO();
        Parser p = new Parser();
        TableBuilder t = new TableBuilder();
        WebController w = new WebController();
        View v = new View();
        QueryController q = new QueryController();
        //DataSet dataset = new DataSet("UpdateCatalog");
        string CATALOG_URL = "https://www.catalog.update.microsoft.com/Search.aspx?q=";
        string OS = "Windows Server 2012 R2";
        string downloadPath = "D:\\WUDownloader\\Downloads";
        string importPath = "D:\\WUDownloader\\Import";
        string tablePath = "D:\\WUDownloader\\Table\\UpdateCatalog";
        public void Run()
        {
            //Import Update List from File
            List<string> lines = f.ImportFileToArray(importPath + "\\Updates.txt"); 

            //Parse Update Titles from imported lines
            List<string> updateTitles = p.ParseLinesContaining(lines, "(KB");
            
            //build table
            t.buildTable(tablePath);

            Console.WriteLine("Attempting to collect data for " + updateTitles.Count + " updates...");
            int x = 0;
            foreach (string updateTitle in updateTitles) //For each update
            {
                Console.WriteLine("Collecting data for update " + (x+1) + " of " + updateTitles.Count + ".");
                string kb = updateTitle.Split('(', ')')[1];
                HtmlDocument siteAsHtml = w.getSiteAsHTML(CATALOG_URL + kb);
                t.populateTable(siteAsHtml, tablePath);
                x++;
            }
            Console.WriteLine("Data collection complete.");


            DownloadManager d = new DownloadManager();
            foreach (string title in updateTitles) //For each update
            {
                string kb = title.Split('(', ')')[1];
                string id = q.getIDFromTable(t.Table, title, OS);
                List<string> downloadUrls = w.getDownloadURLs(id); //gets all download URLs for update at current index
                foreach (string downloadUrl in downloadUrls)
                {
                    DownloadItem downloadItem = new DownloadItem(title, kb, downloadUrl);
                    d.addDownloadItemToQueue(downloadItem);
                }
            }
            Console.WriteLine("Queue loading complete...");
            d.downloadFilesFromQueue(downloadPath);

            Console.WriteLine("Exiting...");
            System.Console.ReadKey();
        }
    }
}
