using System;

namespace WUDownloader
{
    class Configuration
    {
        private static string configurationFilePath = "D:\\WUDownloader\\config.txt";
        //Download Manager non-constants
        private static string downloadPath;
        private static string importPath;
        private static string tablePath;
        private static bool downloadFor_xp;
        private static bool downloadFor_vista;
        private static bool downloadFor_seven;
        private static bool downloadFor_eight;
        private static bool downloadFor_eightOne;
        private static bool downloadFor_ten;
        private static bool downloadFor_server2003;
        private static bool downloadFor_server2008;
        private static bool downloadFor_server2012;
        private static bool downloadFor_server2012R2;
        private static bool downloadFor_server2016;

        //WebController constants
        private static string catalog_url = "https://www.catalog.update.microsoft.com/Search.aspx?q=";
        private static string download_dialog_url = "https://www.catalog.update.microsoft.com/DownloadDialog.aspx";

        //Download Manager constants
        private static string downloadPathPrefix = "downloadPath=";
        private static string importPathPrefix = "importPath=";
        private static string tablePathPrefix = "tablePath=";
        private static string xpPrefix = "downloadForXP=";
        private static string vistaPrefix = "downloadForVista=";
        private static string sevenPrefix = "downloadForSeven=";
        private static string eightPrefix = "downloadForEight=";
        private static string eightOnePrefix = "downloadForEightOne=";
        private static string tenPrefix = "downloadForTen=";
        private static string server2003Prefix = "downloadForServer2003=";
        private static string server2008Prefix = "downloadForServer2008=";
        private static string server2012Prefix = "downloadForServer2012=";
        private static string server2012R2Prefix = "downloadForServer2012R2=";
        private static string server2016Prefix = "downloadForServer2016=";

        public static string CATALOG_URL { get => catalog_url; }
        public static string DownloadPath { get => downloadPath; set => downloadPath = value; }
        public static string ImportPath { get => importPath; set => importPath = value; }
        public static string TablePath { get => tablePath; set => tablePath = value; }
        public static string Download_dialog_url { get => download_dialog_url; set => download_dialog_url = value; }
        public static bool DownloadFor_xp { get => downloadFor_xp; set => downloadFor_xp = value; }
        public static bool DownloadFor_vista { get => downloadFor_vista; set => downloadFor_vista = value; }
        public static bool DownloadFor_seven { get => downloadFor_seven; set => downloadFor_seven = value; }
        public static bool DownloadFor_eight { get => downloadFor_eight; set => downloadFor_eight = value; }
        public static bool DownloadFor_eightOne { get => downloadFor_eightOne; set => downloadFor_eightOne = value; }
        public static bool DownloadFor_ten { get => downloadFor_ten; set => downloadFor_ten = value; }
        public static bool DownloadFor_server2003 { get => downloadFor_server2003; set => downloadFor_server2003 = value; }
        public static bool DownloadFor_server2008 { get => downloadFor_server2008; set => downloadFor_server2008 = value; }
        public static bool DownloadFor_server2012 { get => downloadFor_server2012; set => downloadFor_server2012 = value; }
        public static bool DownloadFor_server2012R2 { get => downloadFor_server2012R2; set => downloadFor_server2012R2 = value; }
        public static bool DownloadFor_server2016 { get => downloadFor_server2016; set => downloadFor_server2016 = value; }
        public static string ConfigurationFilePath { get => configurationFilePath; }

        public static void setDefaultConfiguration()
        {
            downloadPath = "D:\\WUDownloader\\Downloads";
            importPath = "D:\\WUDownloader\\Import";
            tablePath = "D:\\WUDownloader\\Table\\UpdateCatalog";
            DownloadFor_xp = false;
            DownloadFor_vista = false;
            DownloadFor_seven = false;
            DownloadFor_eight = false;
            DownloadFor_eightOne = false;
            DownloadFor_ten = false;
            DownloadFor_server2003 = false;
            DownloadFor_server2008 = false;
            DownloadFor_server2012 = false;
            DownloadFor_server2012R2 = false;
            DownloadFor_server2016 = false;
        }
        public static string[] getCurrentConfiguration()
        {
            string[] configLines = new string[14];
            configLines[0] = downloadPathPrefix + downloadPath;
            configLines[1] = importPathPrefix + importPath;
            configLines[2] = tablePathPrefix + tablePath;
            configLines[3] = xpPrefix + DownloadFor_xp;
            configLines[4] = vistaPrefix + DownloadFor_vista;
            configLines[5] = sevenPrefix + DownloadFor_seven;
            configLines[6] = eightPrefix + DownloadFor_eight;
            configLines[7] = eightOnePrefix + DownloadFor_eightOne;
            configLines[8] = tenPrefix + DownloadFor_ten;
            configLines[9] = server2003Prefix + DownloadFor_server2003;
            configLines[10] = server2008Prefix + DownloadFor_server2008;
            configLines[11] = server2012Prefix + DownloadFor_server2012;
            configLines[12] = server2012R2Prefix + DownloadFor_server2012R2;
            configLines[13] = server2016Prefix + DownloadFor_server2016;
            return configLines;
        }

        public static void setNewConfiguration(Object[] configLines)
        {
            setDefaultConfiguration();
            downloadPath = configLines[0].ToString();
            importPath = configLines[1].ToString();
            tablePath = configLines[2].ToString();
            DownloadFor_xp = (bool)configLines[3];
            DownloadFor_vista = (bool)configLines[4];
            DownloadFor_seven = (bool)configLines[5];
            DownloadFor_eight = (bool)configLines[6];
            DownloadFor_eightOne = (bool)configLines[7];
            DownloadFor_ten = (bool)configLines[8];
            DownloadFor_server2003 = (bool)configLines[9];
            DownloadFor_server2008 = (bool)configLines[10];
            DownloadFor_server2012 = (bool)configLines[11];
            DownloadFor_server2012R2 = (bool)configLines[12];
            DownloadFor_server2016 = (bool)configLines[13];

        }
    }
}
