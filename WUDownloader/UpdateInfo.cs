using System;
using System.Collections.Specialized;

namespace WUDownloader
{
    public class UpdateInfo
    {
        private string id;
        private string title;
        private string product;
        private string classification;
        private DateTime lastUpdated;
        private string version;
        private string size;
        private OrderedDictionary downloadUrls;

        public UpdateInfo(string id, string title, string product, string classification, DateTime lastUpdated, string version, string size, OrderedDictionary downloadUrls)
        {
            Id = id;
            Title = title;
            Product = product;
            Classification = classification;
            LastUpdated = lastUpdated;
            Version = version;
            Size = size;
            DownloadUrls = downloadUrls;
        }

        public string Id { get => id; set => id = value; }
        public string Title { get => title; set => title = value; }
        public string Product { get => product; set => product = value; }
        public string Classification { get => classification; set => classification = value; }
        public DateTime LastUpdated { get => lastUpdated; set => lastUpdated = value; }
        public string Version { get => version; set => version = value; }
        public string Size { get => size; set => size = value; }
        public OrderedDictionary DownloadUrls { get => downloadUrls; set => downloadUrls = value; }

        public object[] ToArray()
        {
            object[] updateInfoArray = {Id, Title, Product, Classification, LastUpdated, Version, Size, DownloadUrls };
            return updateInfoArray;
        }
        public object[] ToArrayAt(int index)
        {
            object[] updateInfoArray = { Id, Title, Product, Classification, LastUpdated, Version, Size, DownloadUrls[index] };
            return updateInfoArray;
        }
    }
}
