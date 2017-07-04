using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading;

namespace WUDownloader
{
    class DownloadManager
    {
        private List<DownloadItem> downloadQueue = new List<DownloadItem>();

        public void addDownloadItemToQueue(DownloadItem downloadItem)
        {
            int queueSize = downloadQueue.Count;
            downloadQueue.Add(downloadItem);
            if (queueSize + 1 == downloadQueue.Count)
            {
                Console.WriteLine("Item added to download queue. Queue Size: " + downloadQueue.Count);
            }
            else
            {
                Console.WriteLine("Item not successfully added to download queue.");
            }
        }
        public void downloadFilesFromQueue(string downloadFolderPath)
        {
            Console.WriteLine("Initializing downloads...");
            Console.WriteLine("Download Queue Size: " + downloadQueue.Count);

            List<DownloadItem> SortedList = downloadQueue.OrderBy(o => o.Kb).ToList();

            for (int x = 0; x < SortedList.Count; x++)
            {
                Console.WriteLine("\nDownloading file for update: " + SortedList[x].Title);
                string downloadUrl = SortedList[x].DownloadUrl;
                Downloader d = new Downloader();
                Console.WriteLine("File #{0} - {1}", x, downloadUrl.Substring(downloadUrl.LastIndexOf('/') + 1));
                d.startDownload(SortedList[x], downloadFolderPath);
            }
        }
    }
}