
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
    }
}
