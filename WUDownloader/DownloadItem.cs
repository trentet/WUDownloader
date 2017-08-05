
namespace WUDownloader
{
    class DownloadItem
    {
        private string title;
        private string kb;
        private string product;
        private string downloadUrl;

        public DownloadItem(string title, string kb, string product, string downloadUrl)
        {
            Title = title;
            Kb = kb;
            DownloadUrl = downloadUrl;
            Product = product;
        }

        public string Title { get => title; set => title = value; }
        public string Kb { get => kb; set => kb = value; }
        public string DownloadUrl { get => downloadUrl; set => downloadUrl = value; }
        public string Product { get => product; set => product = value; }
    }
}
