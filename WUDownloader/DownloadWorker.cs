using System;
using System.ComponentModel;
using System.Net;
using System.Threading;

namespace WUDownloader
{
    class DownloadWorker
    {
        public bool IsCompleted { get; private set; }
        public bool IsDownloading { get; private set; }

        public void StartDownload(DownloadItem downloadItem, string downloadFolderPath)
        {
            IsCompleted = false;
            IsDownloading = false;
            Thread thread = new Thread(() => {
                WebClient client = new WebClient();
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(Client_DownloadProgressChanged);
                client.DownloadFileCompleted += new AsyncCompletedEventHandler(Client_DownloadFileCompleted);
                Uri uri = new Uri(downloadItem.DownloadUrl);
                client.DownloadFileAsync(new Uri(downloadItem.DownloadUrl), downloadFolderPath + "\\" + System.IO.Path.GetFileName(uri.LocalPath));
            });
            thread.Start();
            thread.Join();
            while (IsCompleted == false)
            {
                Thread.Sleep(1000);
            }
            thread.Abort();
        }

        void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            IsDownloading = true;
            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = Math.Round(bytesIn / totalBytes * 100, 2);
            Console.Write("\rDownload status: " + percentage + "%   "); //3 Extra spaces to wipe leftover digits on whole numbers
        }
        void Client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            IsCompleted = true;
            
            Console.WriteLine("\nDownload Completed\n\r");
            IsDownloading = false;
        }
    }
}
