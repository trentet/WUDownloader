using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WUDownloader
{
    class CatalogItem
    {
        string id;
        string title;
        string products;
        string classification;
        string lastUpdated;
        string version;
        string size;
        string buttonHTML;

        public string Id { get => id; set => id = value; }
        public string Title { get => title; set => title = value; }
        public string Products { get => products; set => products = value; }
        public string Classification { get => classification; set => classification = value; }
        public string LastUpdated { get => lastUpdated; set => lastUpdated = value; }
        public string Version { get => version; set => version = value; }
        public string Size { get => size; set => size = value; }
        public string ButtonHTML { get => buttonHTML; set => buttonHTML = value; }
    }
}
