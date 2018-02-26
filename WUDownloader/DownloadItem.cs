
namespace WUDownloader
{
    class DownloadItem
    {
        private string title;
        private string kb;
        private string product;
        private string downloadUrl;
        private string language;

        public DownloadItem(string title, string kb, string product, string downloadUrl, string language)
        {
            Title = title;
            Kb = kb;
            DownloadUrl = downloadUrl;
            Product = product;
            Language = language;
        }

        public string Title { get => title; set => title = value; }
        public string Kb { get => kb; set => kb = value; }
        public string DownloadUrl { get => downloadUrl; set => downloadUrl = value; }
        public string Product { get => product; set => product = value; }
        public string Language { get => language; set => language = value; }
    }
}
