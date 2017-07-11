using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace WUDownloader
{
    class Controller
    {
        TableBuilder t = new TableBuilder();
        string CATALOG_URL = "https://www.catalog.update.microsoft.com/Search.aspx?q=";
        string OS = "Windows Server 2012 R2";
        string downloadPath = "D:\\WUDownloader\\Downloads";
        string importPath = "D:\\WUDownloader\\Import";
        string tablePath = "D:\\WUDownloader\\Table\\UpdateCatalog";
        public static string DOWNLOAD_DIALOG_URL = "https://www.catalog.update.microsoft.com/DownloadDialog.aspx";
        public void Run()
        {
            //Import Update List from File
            List<string> lines = FileIO.ImportFileToArray(importPath + "\\Updates.txt"); 

            //Parse Update Titles from imported lines
            List<string> updateTitles = Parser.ParseLinesContaining(lines, "(KB");

            //Check if file exists
            if (File.Exists(tablePath + ".csv")) //If exists
            {
                Console.WriteLine("CSV file exists. Importing...");
                //Import file
                List<string> csv = FileIO.ImportCsvToStringList(tablePath + ".csv");

                //Build table with schema
                t.buildTableSchema(tablePath);
                //Populate table from file
                t.populateTableFromCsv(csv, true);
            }
            else //If not exists
            {
                Console.WriteLine("CSV file does not exists. Generating...");
                //Build table from scratch
                t.buildTableSchema(tablePath);
                FileIO.ExportDataTableToCSV(t.Table, tablePath);
            }

            Console.WriteLine("Attempting to collect data for " + updateTitles.Count + " updates...");
            int x = 0;
            foreach (string updateTitle in updateTitles) //For each update
            {
                Console.WriteLine("Collecting data for update " + (x+1) + " of " + updateTitles.Count + ".");

                Console.WriteLine("Title is: " + updateTitle);
                
                //If data exists in CSV file
                if (QueryController.doesUpdateTitleExistInTable(t.Table, updateTitle) == true)
                {
                    Console.WriteLine("Update data already exists in table. Skipping...");
                }
                else //Data doesn't exist in CSV file, so collect it and populate the table
                {
                    string kb = updateTitle.Split('(', ')')[1];
                    HtmlDocument siteAsHtml = WebController.getSiteAsHTML(CATALOG_URL + kb);
                    t.populateTableFromSite(siteAsHtml, tablePath);
                }
                x++;
            }
            Console.WriteLine("Data collection complete.");

            DownloadManager d = new DownloadManager();
            foreach (string title in updateTitles) //For each update
            {
                string kb = title.Split('(', ')')[1];
                //gets all download URLs for update at current index
                string[] downloadUrls = QueryController.getDownloadUrlsFromTable(t.Table, title, OS).Split(','); 
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
