using System;
using System.Collections.Generic;
using TLogger.Controller;

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
        private static string[] tableHeaders = new string[] { "id", "title", "product", "classification", "lastUpdated", "version", "size", "downloadUrls", "languages" };
        private static Type[] tableColumnTypes = new Type[] { Type.GetType("System.Guid"), Type.GetType("System.String"), Type.GetType("System.String"),
                                                  Type.GetType("System.String"), Type.GetType("System.DateTime"), Type.GetType("System.String"),
                                                  Type.GetType("System.String"), Type.GetType("System.String"), Type.GetType("System.String")};
        //Download Manager non-constants
        private static string rootFolderPath;
        private static string downloadFolderPath;
        private static string importFolderPath;
        private static string tableFolderPath;
        private static string logFolderPath = "";
        private static string logFileName = "";

        //Download Manager constants
        private static string rootPathPrefix = "rootPath=";
        private static string downloadPathPrefix = "downloadPath=";
        private static string importPathPrefix = "importPath=";
        private static string tablePathPrefix = "tablePath=";
        private static string logPathPrefix = "logPath=";

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
        public static string LogFolderPath { get => logFolderPath; set { logFolderPath = value; Logger.SaveDirectory = LogFolderPath; } }
        public static string LogPathPrefix { get => logPathPrefix; set => logPathPrefix = value; }
        public static string LogFileName { get => logFileName; set { logFileName = value; Logger.FileName = LogFileName; } }

        public static LogHandler Logger = new LogHandler(LogFolderPath, LogFileName);

        public static void SetDefaultConfiguration()
        {
            if (isPortable)
            {
                string executionPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).Replace("file:\\", "");
                RootFolderPath = executionPath;
            }
            else
            {
                RootFolderPath = "C:\\WUDownloader";
                //ConfigurationFolderPath = rootFolderPath + "\\Config";
            }
            ConfigurationFolderPath = rootFolderPath + "\\Config";
            DownloadFolderPath = rootFolderPath + "\\Downloads";
            ImportFolderPath = rootFolderPath + "\\Import";
            TableFolderPath = rootFolderPath + "\\Table";
            LogFolderPath = rootFolderPath + "\\Logs";
            LogFileName = "log";
        }
        public static List<string> GetCurrentConfiguration()
        {
            List<string> configLines = new List<string>
            {
                RootPathPrefix + RootFolderPath,
                DownloadPathPrefix + DownloadFolderPath,
                ImportPathPrefix + ImportFolderPath,
                TablePathPrefix + TableFolderPath,
                LogPathPrefix + LogFolderPath
            };
            return configLines;
        }

        public static void SetNewConfiguration(List<string> configLines)
        {
            SetDefaultConfiguration();
            RootFolderPath = configLines[0];
            DownloadFolderPath = configLines[1];
            ImportFolderPath = configLines[2];
            TableFolderPath = configLines[3];
            LogFolderPath = configLines[4];
            LogFileName = "log";
        }
    }
}
