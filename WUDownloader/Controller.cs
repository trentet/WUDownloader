using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace WUDownloader
{
    class Controller
    {
        TableBuilder t = new TableBuilder();
        public void Run()
        {
            ConfigurationSetup();
            FolderStructureSetup();

            List<string> updateTitles = CollectUpdateTitles();
            if (updateTitles.Count > 0)
            {
                SetupTable();
                CollectUpdateDataForTable(updateTitles);
                StartDownloadManager(updateTitles);
            }
            else
            {
                Console.WriteLine("No updates were found.");
            }

            Console.WriteLine("Exiting...");
            System.Console.ReadKey();
        }
        private List<string> CollectUpdateTitles()
        {
            Console.WriteLine("Would you like to import update titles from file or scan current device?");
            ConsoleInput c = new ConsoleInput();
            int input = 0;
            do
            {
                Console.WriteLine("\nEnter 1 for Import or 2 for Scan: ");
                input = c.getUserInputInteger();
            } while (input != 1 && input != 2);

            List<string> updateTitles = new List<string>();
            if (input == 1)
            {
                Console.WriteLine("\nYou have chosen to Import. Importing...");
                updateTitles = ImportUpdateTitles();
                return updateTitles;
            }
            else if (input == 2)
            {
                Console.WriteLine("\nYou have chosen to Scan. Scanning...");
                updateTitles = WindowsUpdate.GetPendingUpdateTitles();
                return updateTitles;
            }
            else
            {
                Console.WriteLine("Something went wrong. Input equals: '" + input + "'");
                return updateTitles;
            }
        }
        private void FolderStructureSetup()
        {

            if (!Directory.Exists(Configuration.RootPath))
            {
                Configuration.setDefaultConfiguration();
                Console.WriteLine("WUDownload folder structure is missing. Reconstructing using configuration settings...");
                Configuration.setDefaultConfiguration();
                Directory.CreateDirectory(Configuration.RootPath);
                Directory.CreateDirectory(Configuration.DownloadPath);
                Directory.CreateDirectory(Configuration.ImportPath);
                Directory.CreateDirectory(Configuration.TablePath);
                if (Directory.Exists(Configuration.RootPath) && Directory.Exists(Configuration.DownloadPath) &&
                    Directory.Exists(Configuration.ImportPath) && Directory.Exists(Configuration.TablePath))
                {
                    Console.WriteLine("Folder creation successful. Root folder located at: " + Configuration.RootPath);
                }
                else
                {
                    Console.WriteLine("Folder creation failed. Attempted root folder creation at: " + Configuration.RootPath);
                }
            }
        }
        private void ConfigurationSetup()
        {
            Console.WriteLine("Attempting to import configuration file at " + Configuration.ConfigurationFilePath);
            
            if (!Directory.Exists(Configuration.ConfigurationFolderPath)) //Config file is missing
            {
                Directory.CreateDirectory(Configuration.ConfigurationFolderPath);
            }
            if (!File.Exists(Configuration.ConfigurationFilePath))
            {
                Console.WriteLine("Configuration file does not exist. Recreating with default settings.");
                Configuration.setDefaultConfiguration(); //sets default values

                FileIO.ExportStringListToFile(Configuration.ConfigurationFilePath, Configuration.getCurrentConfiguration());
                if (!File.Exists(Configuration.ConfigurationFilePath))
                {
                    Console.WriteLine("Something went wrong. Configuration file not saved.");
                }
                else
                {
                    Console.WriteLine("Configuration file saved successfully.");
                }
            }
            else // Config File exists, import
            {
                Console.WriteLine("Configuration file detected. Importing...");
                List<string> configLines = FileIO.ImportFileToStringList(Configuration.ConfigurationFilePath);
                List<Object> configValues = Parser.parseConfigFile(configLines);
                Configuration.setNewConfiguration(configValues);
                Console.WriteLine("Configuration settings imported.");
            }
        }

        private List<string> ImportUpdateTitles()
        {
            string updatesFilePath = Configuration.ImportPath + "\\Updates.txt";
            List<string> updateTitles = new List<string>();
            Console.WriteLine("Importing update title list from file: " + updatesFilePath);

            if (File.Exists(updatesFilePath))
            {
                //Import Update List from File
                List<string> lines = FileIO.ImportFileToStringList(updatesFilePath);

                //Parse Update Titles from imported lines
                updateTitles = Parser.ParseLinesContaining(lines, "(KB");
                Console.WriteLine(updateTitles.Count + " update titles collected.");
            }
            else
            {
                Console.WriteLine("Update.txt file not found at " + updatesFilePath);
                File.Create(updatesFilePath).Dispose();
                if (File.Exists(updatesFilePath))
                {
                    Console.WriteLine("Updates.txt file created. Please populate it with update information and restart WUDownloader.");

                }
                else
                {
                    Console.WriteLine("Something went wrong. Updates.txt not created at " + updatesFilePath);
                }
                Console.WriteLine("Exiting...");
                System.Console.ReadKey();
                Environment.Exit(0);
            }
            
            return updateTitles;
        }
        private void SetupTable()
        {
            //Check if file exists
            if (File.Exists(Configuration.TablePath + "\\" + Configuration.TableName + ".csv")) //If exists
            {
                Console.WriteLine("CSV file exists. Importing...");
                //Import file
                List<string> csv = FileIO.ImportCsvToStringList(Configuration.TablePath + "\\" + Configuration.TableName);

                //Build table with schema
                t.buildTableSchema();
                //Populate table from file
                t.populateTableFromCsv(csv, true);
            }
            else //If not exists
            {
                Console.WriteLine("CSV file does not exists. Generating...");
                //Build table from scratch
                t.buildTableSchema();
                FileIO.ExportDataTableToCSV(TableBuilder.Table, Configuration.TablePath, Configuration.TableName);
                Console.WriteLine("CSV file saved.");
            }
        }
        private void CollectUpdateDataForTable(List<string> updateTitles)
        {
            Console.WriteLine("Attempting to collect data for " + updateTitles.Count + " updates...");
            int x = 0;
            foreach (string updateTitle in updateTitles) //For each update
            {
                Console.WriteLine("Collecting data for update " + (x + 1) + " of " + updateTitles.Count + ".");

                Console.WriteLine("Title is: " + updateTitle);

                //If data exists in CSV file
                if (QueryController.doesUpdateTitleExistInTable(TableBuilder.Table, updateTitle) == true)
                {
                    Console.WriteLine("Update data already exists in table. Skipping...");
                }
                else //Data doesn't exist in CSV file, so collect it and populate the table
                {
                    string kb = updateTitle.Split('(', ')')[1];
                    HtmlDocument siteAsHtml = WebController.getSiteAsHTML(Configuration.CATALOG_URL + kb);
                    t.populateTableFromSite(siteAsHtml, Configuration.TablePath, Configuration.TableName);
                }
                x++;
            }
            Console.WriteLine("Data collection complete.");
        }
        private void StartDownloadManager(List<string> updateTitles)
        {
            DownloadManager d = new DownloadManager();
            d.setOsList();

            Console.WriteLine("Populating download queue...");
            d.populateDownloadQueue(updateTitles);
            Console.WriteLine("Queue loading complete...");
            Console.WriteLine("Initializing download sequence...");
            d.downloadFilesFromQueue();
            Console.WriteLine("Downloads complete.");
        }
    }
}
