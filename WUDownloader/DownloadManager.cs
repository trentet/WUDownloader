using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WUDownloader
{
    class DownloadManager
    {
        private List<DownloadItem> downloadQueue = new List<DownloadItem>();

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
        public void downloadFilesFromQueue(string downloadFolderPath)
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
                if (File.Exists(downloadFolderPath + "\\" + System.IO.Path.GetFileName(uri.LocalPath))) //Checks if file is already downloaded
                {
                    Console.WriteLine("File already exists. Skipping...");
                }
                else //File is not already downloaded. Download.
                {
                    DownloadWorker d = new DownloadWorker();
                    d.startDownload(SortedDownloadQueue[x], downloadFolderPath);
                }
            }
        }
    }
}