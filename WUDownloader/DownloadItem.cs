
namespace WUDownloader
{
    class DownloadItem
    {
        private string title;
        private string kb;
        private string os;
        private string downloadUrl;

        public DownloadItem(string title, string kb, string os, string downloadUrl)
        {
            Title = title;
            Kb = kb;
            DownloadUrl = downloadUrl;
            Os = os;
        }

        public string Title { get => title; set => title = value; }
        public string Kb { get => kb; set => kb = value; }
        public string DownloadUrl { get => downloadUrl; set => downloadUrl = value; }
        public string Os { get => os; set => os = value; }
    }
}
