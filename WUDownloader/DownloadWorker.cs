using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WUDownloader
{
    class DownloadWorker
    {
        private bool isCompleted;
        private bool isDownloading;


        public bool IsCompleted { get => isCompleted; set => isCompleted = value; }
        public bool IsDownloading { get => isDownloading; set => isDownloading = value; }

        public void startDownload(DownloadItem downloadItem, string downloadFolderPath)
        {
            isCompleted = false;
            isDownloading = false;
            Thread thread = new Thread(() => {
                WebClient client = new WebClient();
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                Uri uri = new Uri(downloadItem.DownloadUrl);
                client.DownloadFileAsync(new Uri(downloadItem.DownloadUrl), downloadFolderPath + "\\" + System.IO.Path.GetFileName(uri.LocalPath));
            });
            thread.Start();
            thread.Join();
            while (isCompleted == false)
            {
                Thread.Sleep(1000);
            }
            thread.Abort();
        }

        void client_DownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {
            isDownloading = true;
            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = Math.Round(bytesIn / totalBytes * 100, 2);
            Console.Write("\rDownload status: " + percentage + "%");
        }
        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            isCompleted = true;
            
            Console.WriteLine("\nDownload Completed\n\r");
            isDownloading = false;
        }
    }
}
