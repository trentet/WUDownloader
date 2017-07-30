using System;
using System.Collections.Generic;

namespace WUDownloader
{
    class Configuration
    {
        private static string configurationFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\WUDownloader";
        private static string configurationFilePath = ConfigurationFolderPath + "\\config.txt";
        private static string tableName = "UpdateCatalog";
        
        //Download Manager non-constants
        private static string rootPath;
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
        private static string rootPathPrefix = "rootPath=";
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
        public static string RootPath { get => rootPath; set => rootPath = value; }
        public static string TableName { get => tableName; }
        public static string ConfigurationFolderPath { get => configurationFolderPath; }

        public static void setDefaultConfiguration()
        {
            rootPath = "C:\\WUDownloader";
            downloadPath = rootPath + "\\Downloads";
            importPath = rootPath + "\\Import";
            tablePath = rootPath + "\\Table";
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
        public static List<string> getCurrentConfiguration()
        {
            List<string> configLines = new List<string>();
            configLines.Add(rootPathPrefix + RootPath);
            configLines.Add(downloadPathPrefix + downloadPath);
            configLines.Add(importPathPrefix + importPath);
            configLines.Add(tablePathPrefix + tablePath);
            configLines.Add(xpPrefix + DownloadFor_xp);
            configLines.Add(vistaPrefix + DownloadFor_vista);
            configLines.Add(sevenPrefix + DownloadFor_seven);
            configLines.Add(eightPrefix + DownloadFor_eight);
            configLines.Add(eightOnePrefix + DownloadFor_eightOne);
            configLines.Add(tenPrefix + DownloadFor_ten);
            configLines.Add(server2003Prefix + DownloadFor_server2003);
            configLines.Add(server2008Prefix + DownloadFor_server2008);
            configLines.Add(server2012Prefix + DownloadFor_server2012);
            configLines.Add(server2012R2Prefix + DownloadFor_server2012R2);
            configLines.Add(server2016Prefix + DownloadFor_server2016);
            return configLines;
        }

        public static void setNewConfiguration(List<Object> configLines)
        {
            setDefaultConfiguration();
            RootPath = configLines[0].ToString();
            DownloadPath = configLines[1].ToString();
            ImportPath = configLines[2].ToString();
            TablePath = configLines[3].ToString();
            DownloadFor_xp = (bool)configLines[4];
            DownloadFor_vista = (bool)configLines[5];
            DownloadFor_seven = (bool)configLines[6];
            DownloadFor_eight = (bool)configLines[7];
            DownloadFor_eightOne = (bool)configLines[8];
            DownloadFor_ten = (bool)configLines[9];
            DownloadFor_server2003 = (bool)configLines[10];
            DownloadFor_server2008 = (bool)configLines[11];
            DownloadFor_server2012 = (bool)configLines[12];
            DownloadFor_server2012R2 = (bool)configLines[13];

            DownloadFor_server2016 = (bool)configLines[14];
        }
    }
}
