using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WUDownloader
{
    class DownloadItem
    {
        private string title;
        private string kb;
        private string downloadUrl;
        private bool isDownloaded;
        private bool isDownloading;

        public DownloadItem(string title, string kb, string downloadUrl)
        {
            Title = title;
            Kb = kb;
            DownloadUrl = downloadUrl;
            IsDownloaded = false;
            IsDownloading = false;
        }

        public string Title { get => title; set => title = value; }
        public string Kb { get => kb; set => kb = value; }
        public string DownloadUrl { get => downloadUrl; set => downloadUrl = value; }
        public bool IsDownloaded { get => isDownloaded; set => isDownloaded = value; }
        public bool IsDownloading { get => isDownloading; set => isDownloading = value; }


        //public void client_DownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
        //{
        //    downloading = true;
        //    Thread.Sleep(1000);
        //    double bytesIn = double.Parse(e.BytesReceived.ToString());
        //    double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
        //    double percentage = Math.Round(bytesIn / totalBytes * 100, 2);
        //    Console.Write("\rDownload status: " + percentage + "%");
        //}
        //public void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        //{
        //    Console.Write("\rDownload Completed\n\r");
        //    Thread.Sleep(100);
        //    downloaded = true;
        //    downloading = false;
        //}
    }
}
