using System;
using System.Collections.Generic;

namespace WUDownloader
{
    class Configuration
    {
        private static bool isPortable = false;
        private static string configurationFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\WUDownloader";
        private static string configurationFilePath;
        private static string tableName = "UpdateCatalog";
        private static Dictionary<string, Type> schemaDictionary = new Dictionary<string, Type>();
        
        //Download Manager non-constants
        private static string rootPath;
        private static string downloadPath;
        private static string importPath;
        private static string tablePath;

        //Download Manager constants
        private static string rootPathPrefix = "rootPath=";
        private static string downloadPathPrefix = "downloadPath=";
        private static string importPathPrefix = "importPath=";
        private static string tablePathPrefix = "tablePath=";

        //WebController constants
        private static string catalog_url = "https://www.catalog.update.microsoft.com/Search.aspx?q=";
        private static string download_dialog_url = "https://www.catalog.update.microsoft.com/DownloadDialog.aspx";

        public static string CATALOG_URL { get => catalog_url; }
        public static string DownloadPath { get => downloadPath; set => downloadPath = value; }
        public static string ImportPath { get => importPath; set => importPath = value; }
        public static string TablePath { get => tablePath; set => tablePath = value; }
        public static string Download_dialog_url { get => download_dialog_url; set => download_dialog_url = value; }
        public static string RootPath { get => rootPath; set => rootPath = value; }
        public static string TableName { get => tableName; }
        public static string ConfigurationFolderPath { get => configurationFolderPath; set { configurationFolderPath = value; ConfigurationFilePath = configurationFolderPath + "\\config.txt"; } }
        public static string ConfigurationFilePath { get => configurationFilePath; set => configurationFilePath = value; }
        public static string RootPathPrefix { get => rootPathPrefix; set => rootPathPrefix = value; }
        public static string DownloadPathPrefix { get => downloadPathPrefix; set => downloadPathPrefix = value; }
        public static string ImportPathPrefix { get => importPathPrefix; set => importPathPrefix = value; }
        public static string TablePathPrefix { get => tablePathPrefix; set => tablePathPrefix = value; }
        public static bool IsPortable { get => isPortable; set => isPortable = value; }
        public static Dictionary<string, Type> SchemaDictionary { get => schemaDictionary; set => schemaDictionary = value; }

        public static void setDefaultConfiguration()
        {
            if (isPortable)
            {
                string executionPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).Replace("file:\\", "");
                RootPath = executionPath;
            }
            else
            {
                RootPath = "C:\\WUDownloader";
            }
            DownloadPath = rootPath + "\\Downloads";
            ImportPath = rootPath + "\\Import";
            TablePath = rootPath + "\\Table";
        }
        public static List<string> getCurrentConfiguration()
        {
            List<string> configLines = new List<string>();
            configLines.Add(RootPathPrefix + RootPath);
            configLines.Add(DownloadPathPrefix + downloadPath);
            configLines.Add(ImportPathPrefix + importPath);
            configLines.Add(TablePathPrefix + tablePath);
            return configLines;
        }

        public static void setNewConfiguration(List<Object> configLines)
        {
            setDefaultConfiguration();
            RootPath = configLines[0].ToString();
            DownloadPath = configLines[1].ToString();
            ImportPath = configLines[2].ToString();
            TablePath = configLines[3].ToString();
        }
    }
}
