using System;
using System.Collections.Generic;

namespace WUDownloader
{
    public class Configuration
    {
        private static bool isPortable = false;
        private static string configurationFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\WUDownloader";
        private static string configurationFilePath;
        private static string tableName = "UpdateCatalog";
        private static Dictionary<string, Type> schemaDictionary = new Dictionary<string, Type>();

        //Table variables
        private static string[] tableHeaders = new string[] { "id", "title", "product", "classification", "lastUpdated", "version", "size", "downloadUrls" };
        private static Type[] tableColumnTypes = new Type[] { Type.GetType("System.String"), Type.GetType("System.String"), Type.GetType("System.String"),
                                                  Type.GetType("System.String"), Type.GetType("System.DateTime"), Type.GetType("System.String"),
                                                  Type.GetType("System.String"), Type.GetType("System.String")};
        //Download Manager non-constants
        private static string rootFolderPath;
        private static string downloadFolderPath;
        private static string importFolderPath;
        private static string tableFolderPath;

        //Download Manager constants
        private static string rootPathPrefix = "rootPath=";
        private static string downloadPathPrefix = "downloadPath=";
        private static string importPathPrefix = "importPath=";
        private static string tablePathPrefix = "tablePath=";

        //WebController constants
        private static string catalog_url = "https://www.catalog.update.microsoft.com/Search.aspx?q=";
        private static string download_dialog_url = "https://www.catalog.update.microsoft.com/DownloadDialog.aspx";

        public static string CATALOG_URL { get => catalog_url; }
        public static string DownloadFolderPath { get => downloadFolderPath; set => downloadFolderPath = value; }
        public static string ImportFolderPath { get => importFolderPath; set => importFolderPath = value; }
        public static string TableFolderPath { get => tableFolderPath; set => tableFolderPath = value; }
        public static string Download_dialog_url { get => download_dialog_url; set => download_dialog_url = value; }
        public static string RootFolderPath { get => rootFolderPath; set => rootFolderPath = value; }
        public static string TableName { get => tableName; }
        public static string ConfigurationFolderPath { get => configurationFolderPath; set { configurationFolderPath = value; ConfigurationFilePath = configurationFolderPath + "\\config.txt"; } }
        public static string ConfigurationFilePath { get => configurationFilePath; set => configurationFilePath = value; }
        public static string RootPathPrefix { get => rootPathPrefix; set => rootPathPrefix = value; }
        public static string DownloadPathPrefix { get => downloadPathPrefix; set => downloadPathPrefix = value; }
        public static string ImportPathPrefix { get => importPathPrefix; set => importPathPrefix = value; }
        public static string TablePathPrefix { get => tablePathPrefix; set => tablePathPrefix = value; }
        public static bool IsPortable { get => isPortable; set => isPortable = value; }
        public static Dictionary<string, Type> SchemaDictionary { get => schemaDictionary; set => schemaDictionary = value; }
        public static string[] TableHeaders { get => tableHeaders; set => tableHeaders = value; }
        public static Type[] TableColumnTypes { get => tableColumnTypes; set => tableColumnTypes = value; }

        public static void setDefaultConfiguration()
        {
            if (isPortable)
            {
                string executionPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).Replace("file:\\", "");
                RootFolderPath = executionPath;
            }
            else
            {
                RootFolderPath = "C:\\WUDownloader";
                ConfigurationFolderPath = rootFolderPath + "\\config";
            }
            
            DownloadFolderPath = rootFolderPath + "\\Downloads";
            ImportFolderPath = rootFolderPath + "\\Import";
            TableFolderPath = rootFolderPath + "\\Table";
        }
        public static List<string> getCurrentConfiguration()
        {
            List<string> configLines = new List<string>();
            configLines.Add(RootPathPrefix + RootFolderPath);
            configLines.Add(DownloadPathPrefix + downloadFolderPath);
            configLines.Add(ImportPathPrefix + importFolderPath);
            configLines.Add(TablePathPrefix + tableFolderPath);
            return configLines;
        }

        public static void setNewConfiguration(List<Object> configLines)
        {
            setDefaultConfiguration();
            RootFolderPath = configLines[0].ToString();
            DownloadFolderPath = configLines[1].ToString();
            ImportFolderPath = configLines[2].ToString();
            TableFolderPath = configLines[3].ToString();
        }
    }
}
