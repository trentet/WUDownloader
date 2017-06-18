using System;
using System.ComponentModel;
using System.Net;
using System.Threading;

namespace WUDownloader
{
    class Downloader
    {
        private bool isDownloadComplete;

        public bool IsDownloadComplete { get => isDownloadComplete; set => isDownloadComplete = value; }

        public void startDownload(string url, string downloadFolderPath)
        {
            IsDownloadComplete = false;
            Thread thread = new Thread(() => {
                WebClient client = new WebClient();
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                client.DownloadFileAsync(new Uri(url), downloadFolderPath + url.Substring(url.LastIndexOf('/')));
            });
            thread.Start();
            while (IsDownloadComplete == false)
            {

            }
            thread.Abort();
        }
        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = Math.Round(bytesIn / totalBytes * 100, 2);
            Console.Write("\rDownload status: " + percentage + "%");
        }
        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Console.Write(" - Completed\n");
            IsDownloadComplete = true;
        }
    }
}
