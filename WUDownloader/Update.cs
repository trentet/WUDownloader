using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WUDownloader
{
    class UpdateInfo
    {
        private string name;
        private string kb;
        private string catalogURL;
        private string downloadURL;

        public UpdateInfo(string name, string kb, string catalogURL)
        {
            this.name = name;
            this.kb = kb;
            this.catalogURL = catalogURL;
            this.downloadURL = "";
        }

        public string Name { get => name; set => name = value; }
        public string Kb { get => kb; set => kb = value; }
        public string CatalogURL { get => catalogURL; set => catalogURL = value; }
        public string DownloadURL { get => downloadURL; set => downloadURL = value; }
    }
}
