
namespace WUDownloader
{
    class DownloadItem
    {
        private string title;
        private string kb;
        private string downloadUrl;

        public DownloadItem(string title, string kb, string downloadUrl)
        {
            Title = title;
            Kb = kb;
            DownloadUrl = downloadUrl;
        }

        public string Title { get => title; set => title = value; }
        public string Kb { get => kb; set => kb = value; }
        public string DownloadUrl { get => downloadUrl; set => downloadUrl = value; }
    }
}
