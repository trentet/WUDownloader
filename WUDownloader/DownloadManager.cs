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
        //private enum OperatingSystems
        //{
        //    [Description("Windows XP")] XP = 0,
        //    [Description("Windows Vista")] VISTA = 1,
        //    [Description("Windows 7")] SEVEN = 2,
        //    [Description("Windows 8")] EIGHT = 3,
        //    [Description("Windows 8.1")] EIGHT_ONE = 4,
        //    [Description("Windows 10")] TEN = 5,
        //    [Description("Windows Server 2003")] SERVER_2003 = 6,
        //    [Description("Windows Server 2008")] SERVER_2008 = 7,
        //    [Description("Windows Server 2012")] SERVER_2012 = 8,
        //    [Description("Windows Server 2012 R2")] SERVER_2012_R2 = 9,
        //    [Description("Windows Server 2016")] SERVER_2016 = 10

        //}
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
                List<string>[] products_and_downloadUrls = QueryController.getDownloadUrlsFromTable(TableBuilder.Table, title, productList);
                List<string> productsFromEachRow = products_and_downloadUrls[0];
                List<string> downloadUrlsFromEachRow = products_and_downloadUrls[1];
                for (int x = 0; x < downloadUrlsFromEachRow.Count; x++)
                {
                    string[] downloadUrls = downloadUrlsFromEachRow[x].Split(',');
                    string product = productsFromEachRow[x];
                    foreach (string downloadUrl in downloadUrls)
                    {
                        DownloadItem downloadItem = new DownloadItem(title, kb, product, downloadUrl);
                        addDownloadItemToQueue(downloadItem);
                    }
                }
            }
        }

        //public void setOsList()
        //{
        //    if (osList.Count > 0)
        //    {
        //        osList.Clear();
        //    }
        //    if (Configuration.DownloadFor_xp)
        //    {
        //        osList.Add(EnumExtensions.GetDescription(OperatingSystems.XP));
        //    }
        //    if (Configuration.DownloadFor_vista)
        //    {
        //        osList.Add(EnumExtensions.GetDescription(OperatingSystems.VISTA));
        //    }
        //    if (Configuration.DownloadFor_seven)
        //    {
        //        osList.Add(EnumExtensions.GetDescription(OperatingSystems.SEVEN));
        //    }
        //    if (Configuration.DownloadFor_eight)
        //    {
        //        osList.Add(EnumExtensions.GetDescription(OperatingSystems.EIGHT));
        //    }
        //    if (Configuration.DownloadFor_eightOne)
        //    {
        //        osList.Add(EnumExtensions.GetDescription(OperatingSystems.EIGHT_ONE));
        //    }
        //    if (Configuration.DownloadFor_ten)
        //    {
        //        osList.Add(EnumExtensions.GetDescription(OperatingSystems.TEN));
        //    }
        //    if (Configuration.DownloadFor_server2003)
        //    {
        //        osList.Add(EnumExtensions.GetDescription(OperatingSystems.SERVER_2003));
        //    }
        //    if (Configuration.DownloadFor_server2008)
        //    {
        //        osList.Add(EnumExtensions.GetDescription(OperatingSystems.SERVER_2008));
        //    }
        //    if (Configuration.DownloadFor_server2012)
        //    {
        //        osList.Add(EnumExtensions.GetDescription(OperatingSystems.SERVER_2012));
        //    }
        //    if (Configuration.DownloadFor_server2012R2)
        //    {
        //        osList.Add(EnumExtensions.GetDescription(OperatingSystems.SERVER_2012_R2));
        //    }
        //    if (Configuration.DownloadFor_server2016)
        //    {
        //        osList.Add(EnumExtensions.GetDescription(OperatingSystems.SERVER_2016));
        //    }
        //}
    }
    //static class EnumExtensions
    //{
    //    public static string GetDescription(this Enum value)
    //    {
    //        Type type = value.GetType();
    //        string name = Enum.GetName(type, value);
    //        if (name != null)
    //        {
    //            FieldInfo field = type.GetField(name);
    //            if (field != null)
    //            {
    //                DescriptionAttribute attr =
    //                       Attribute.GetCustomAttribute(field,
    //                         typeof(DescriptionAttribute)) as DescriptionAttribute;
    //                if (attr != null)
    //                {
    //                    return attr.Description;
    //                }
    //            }
    //        }
    //        return null;
    //    }
    //}
}