using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WUDownloader
{
    class DownloadObj
    {
        private string title;
        private string kb;
        private List<string> downloadURLs;

        public DownloadObj(string title, string kb, List<string> downloadURLs)
        {
            this.title = title;
            this.kb = kb;
            this.downloadURLs = downloadURLs;
        }

        public string Title { get => title; set => title = value; }
        public string Kb { get => kb; set => kb = value; }
        public List<string> DownloadURLs { get => downloadURLs; set => downloadURLs = value; }
    }
}
