using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Data;
using TableBuilderLibrary;

namespace WUDownloader
{
    class Controller
    {
        public void Run()
        {
            ConfigurationSetup();
            FolderStructureSetup();
            int menuSelection = selectMode();

            List<string> updateTitles = CollectUpdateTitles();
            if (menuSelection == 1 || menuSelection == 3) //Collect update information
            {
                CollectInfoForTable(updateTitles);
            }
            if (menuSelection == 2 || menuSelection == 3) //Download Updates
            {
                DataTable table = FileIO.ImportTableFromCsv();
                StartDownloadManager(updateTitles, table);
            }
            //else if (menuSelection == 3) // Collect update information and download updates
            //{
            //    List<string> updateTitles = CollectUpdateTitles();
            //    StartDownloadManager(updateTitles);
            //}


            

            Console.WriteLine("Exiting...");
            System.Console.ReadKey();
        }

        public List<string> CollectInfoForTable(List<string> updateTitles)
        {
            if (updateTitles.Count > 0)
            {
                CollectUpdateDataForTable(updateTitles, FileIO.ImportTableFromCsv());
            }
            else
            {
                Console.WriteLine("No updates were found.");
            }
            return updateTitles;
        }
        public int selectMode()
        {
            Console.WriteLine("Select an option below:");
            Console.WriteLine("1. Collect Update Information");
            Console.WriteLine("2. Download Updates");
            Console.WriteLine("3. Collect Update Information & Download Updates");

            ConsoleInput c = new ConsoleInput();
            int input = 0;
            while (input < 1 || input > 3)
            {
                input = c.getUserInputInteger();
                if (input < 1 || input > 3)
                {
                    Console.WriteLine("Incorrect selection. Please try again.");
                }
            }
            Console.WriteLine("");
            return input;
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
            if (!Directory.Exists(Configuration.RootFolderPath))
            {
                Console.WriteLine("WUDownload folder structure is missing. Reconstructing using configuration settings...");
                Directory.CreateDirectory(Configuration.RootFolderPath);
            }
            if (Directory.Exists(Configuration.RootFolderPath))
            {
                if (!Directory.Exists(Configuration.DownloadFolderPath))
                {
                    Directory.CreateDirectory(Configuration.DownloadFolderPath);
                }
                if (!Directory.Exists(Configuration.ImportFolderPath))
                {
                    Directory.CreateDirectory(Configuration.ImportFolderPath);
                }
                if (!Directory.Exists(Configuration.TableFolderPath))
                {
                    Directory.CreateDirectory(Configuration.TableFolderPath);
                }
            }
            if (Directory.Exists(Configuration.RootFolderPath) && Directory.Exists(Configuration.DownloadFolderPath) &&
                    Directory.Exists(Configuration.ImportFolderPath) && Directory.Exists(Configuration.TableFolderPath))
            {
                Console.WriteLine("Folder creation successful. Root folder located at: " + Configuration.RootFolderPath);
            }
            else
            {
                Console.WriteLine("Folder creation failed. Attempted root folder creation at: " + Configuration.RootFolderPath);
            }
        }
        private void ConfigurationSetup()
        {
            string executionPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).Replace("file:\\","");
            //Console.WriteLine(executionPath);
            if (File.Exists(executionPath + "\\" + "portable.txt"))
            {
                Configuration.IsPortable = true;
                Configuration.ConfigurationFolderPath = executionPath;
                //Configuration.ConfigurationFilePath = Configuration.ConfigurationFolderPath + "\\" + "config.txt";
            }

            Configuration.setDefaultConfiguration(); //sets default values

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
                List<Object> configValues = Parser.ParseConfigFile(configLines);
                Configuration.setNewConfiguration(configValues);
                Console.WriteLine("Configuration settings imported.");
            }
        }

        private List<string> ImportUpdateTitles()
        {
            string updatesFilePath = Configuration.ImportFolderPath + "\\Updates.txt";
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
        
        private void CollectUpdateDataForTable(List<string> updateTitles, DataTable table)
        {
            Console.WriteLine("Attempting to collect data for " + updateTitles.Count + " updates...");
            int x = 0;
            foreach (string updateTitle in updateTitles) //For each update
            {
                Console.WriteLine("Collecting data for update " + (x + 1) + " of " + updateTitles.Count + ".");

                Console.WriteLine("Title is: " + updateTitle);

                //If data exists in CSV file
                if (QueryController.doesUpdateTitleExistInTable(table, updateTitle) == true)
                {
                    Console.WriteLine("Update data already exists in table. Skipping...");
                }
                else //Data doesn't exist in CSV file, so collect it and populate the table
                {
                    string kb = Parser.GetKbFromTitle(updateTitle);

                    if (kb.Length > 0)
                    {
                        HtmlDocument siteAsHtml = WebController.getSiteAsHTML(Configuration.CATALOG_URL + kb);
                        table.PopulateTableFromSite(siteAsHtml, Configuration.TableFolderPath, Configuration.TableName);
                    }
                }
                x++;
            }
            Console.WriteLine("Data collection complete.");
        }
        private void StartDownloadManager(List<string> updateTitles, DataTable table)
        {
            List<string> productList = GetProductList(table);
            DownloadManager d = new DownloadManager(productList);
            
            Console.WriteLine("Populating download queue...");
            d.PopulateDownloadQueue(updateTitles, table);
            Console.WriteLine("Queue loading complete...");
            Console.WriteLine("Initializing download sequence...");
            d.downloadFilesFromQueue();
            Console.WriteLine("Downloads complete.");
        }
        public List<string> GetProductList(DataTable table)
        {
            string columnName = "product";
            var productsFromTable = TableBuilder.GetAllDataFromColumn(table, columnName);
            List<string> productList = new List<string>();
            for (int x = 0; x < productsFromTable.Count; x++)
            {
                string productsAtCurrentRow = (string)productsFromTable[x];
                string[] splitProducts = productsAtCurrentRow.Split(',');
                foreach (string product in splitProducts)
                {
                    string trimmedProduct = product.Trim();
                    if (!productList.Contains(trimmedProduct))
                    {
                        productList.Add(trimmedProduct);
                    }
                }

            }

            return productList;
        }
    }
}
