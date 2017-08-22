using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

namespace WUDownloader
{
    
    class DownloadManager
    {
        public DownloadManager(List<string> productList)
        {
            ProductList = productList;
        }

        private List<DownloadItem> downloadQueue = new List<DownloadItem>();
        private List<string> productList = new List<string>();

        public List<string> ProductList { get => productList; set => productList = value; }

        public void addDownloadItemToQueue(DownloadItem downloadItem)
        {
            int queueSize = downloadQueue.Count;
            downloadQueue.Add(downloadItem);
            if (queueSize + 1 == downloadQueue.Count) //Checks if download queue size increased
            {
                Console.WriteLine("Item added to download queue. Queue Size: " + downloadQueue.Count);
            }
            else //Queue size did not increase, therefore item was not added successfully
            {
                Console.WriteLine("Item not successfully added to download queue.");
            }
        }
        public void downloadFilesFromQueue()
        {
            Console.WriteLine("Initializing downloads...");
            Console.WriteLine("Download Queue Size: " + downloadQueue.Count);

            List<DownloadItem> SortedDownloadQueue = downloadQueue.OrderBy(o => o.Kb).ToList();

            for (int x = 0; x < SortedDownloadQueue.Count; x++) //Runs through sorted download queue
            {
                string downloadUrl = SortedDownloadQueue[x].DownloadUrl;
                Uri uri = new Uri(downloadUrl);
                string fileName = System.IO.Path.GetFileName(uri.LocalPath);
                Console.WriteLine("\nDownloading file for update: " + SortedDownloadQueue[x].Title);
                Console.WriteLine("File #{0} - {1}", (x+1), fileName); //{0} is current index, {1} is download's file name
                string[] productList = SortedDownloadQueue[x].Product.Split(',');
                for (int z = 0; z < productList.Length; z++)
                {
                    productList[z] = productList[z].Trim();
                }
                for (int y = 0; y < productList.Length; y++)
                {
                    string downloadFolderPath = Configuration.DownloadPath + "\\" + productList[y] + "\\" + SortedDownloadQueue[x].Title;
                    string fullFilePath = downloadFolderPath + "\\" + System.IO.Path.GetFileName(uri.LocalPath);
                    if (fullFilePath.Length > 260)
                    {
                        int overflow = fullFilePath.Length - 260;
                        string downloadFolderPathWithoutKB = downloadFolderPath.Split('(').First();
                        downloadFolderPath = downloadFolderPathWithoutKB.Substring(0, downloadFolderPathWithoutKB.Length - overflow - 10) + " (" + SortedDownloadQueue[x].Kb + ")";
                        fullFilePath = downloadFolderPath + "\\" + System.IO.Path.GetFileName(uri.LocalPath);
                        int test = fullFilePath.Length;
                    }
                    if (File.Exists(fullFilePath))//Checks if file is already downloaded
                    {
                        Console.WriteLine("File already exists. Skipping...");
                    }
                    else //File is not already downloaded. Download.
                    {
                        //See if current os list contains an OS from the oslist from config, then create folders and download files
                        if (productList.Contains(productList[y]))
                        {
                            Console.WriteLine("Download Path: " + downloadFolderPath);
                            Console.WriteLine("Downloading for " + productList[y]);
                            System.IO.Directory.CreateDirectory(downloadFolderPath);
                            DownloadWorker d = new DownloadWorker();
                            d.startDownload(SortedDownloadQueue[x], downloadFolderPath);
                        }
                    }
                }
            }
        }
        public void populateDownloadQueue(List<string> updateTitles)
        {
            foreach (string title in updateTitles) //For each update
            {
                string kb = title.Split('(', ')')[1];
                //gets all download URLs for update at current index
                //List<string>[] products_and_downloadUrls = QueryController.getDownloadUrlsFromTable(TableBuilder.Table, title, productList);
                //List<string> productsFromEachRow = products_and_downloadUrls[0];
                //List<string> downloadUrlsFromEachRow = products_and_downloadUrls[1];
                //for (int x = 0; x < downloadUrlsFromEachRow.Count; x++)
                //{
                //    string[] downloadUrls = downloadUrlsFromEachRow[x].Split(',');
                //    string product = productsFromEachRow[x];
                //    foreach (string downloadUrl in downloadUrls)
                //    {
                //        DownloadItem downloadItem = new DownloadItem(title, kb, product, downloadUrl);
                //        addDownloadItemToQueue(downloadItem);
                //    }
                //}
            }
        }
    }
}