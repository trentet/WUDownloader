using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Data;
using TableBuilderLibrary;
using System.Collections.Specialized;

namespace WUDownloader
{
    class Controller
    {
        public void Run()
        {
            ConfigurationSetup();
            FolderStructureSetup();
            int menuSelection = SelectMode();

            List<string> updateTitles = CollectUpdateTitles();
            if (menuSelection == 1) //Collect update information
            {
                CollectInfoForTable(updateTitles);
            }
            else if (menuSelection == 2) //Download Updates
            {
                List<UpdateInfo> updates = CollectInfoForTable(updateTitles);
                DataTable table = FileIO.ImportTableFromCsv();
                StartDownloadManager(updates, table);
            }
            //else if (menuSelection == 3) // Collect update information and download updates
            //{
            //    List<string> updateTitles = CollectUpdateTitles();
            //    StartDownloadManager(updateTitles);
            //}




            Console.WriteLine("Exiting...");
            System.Console.ReadKey();
        }

        public List<UpdateInfo> CollectInfoForTable(List<string> updateTitles)
        {
            List<UpdateInfo> updates = new List<UpdateInfo>();
            if (updateTitles.Count > 0)
            {
                updates = CollectUpdateDataForTable(updateTitles, FileIO.ImportTableFromCsv());
                Console.WriteLine("Data collection complete.");
            }
            else
            {
                Console.WriteLine("Update list is empty. ");
            }
            return updates;
        }
        public int SelectMode()
        {
            Console.WriteLine("Select an option below:");
            Console.WriteLine("1. Collect Update Information");
            //Console.WriteLine("2. Download Updates");
            Console.WriteLine("2. Collect Update Information & Download Updates");

            //ConsoleInput c = new ConsoleInput();
            int input = 0;
            do
            {
                input = ConsoleInput.PositiveInteger();
                if (input < 1 || input > 2)
                {
                    Console.WriteLine("Incorrect selection. Please try again.");
                }
            } while (input < 1 || input > 2);
            //while (input != 1 || input > 2)
            //{
            //    input = ConsoleInput.PositiveInteger();
            //    if (input < 1 || input > 2)
            //    {
            //        Console.WriteLine("Incorrect selection. Please try again.");
            //    }
            //}
            Console.WriteLine("");
            return input;
        }
        private List<string> CollectUpdateTitles()
        {
            Console.WriteLine("Would you like to import update titles from file or scan current device?");
            //ConsoleInput c = new ConsoleInput();
            int importOrScanInput = 0;
            do
            {
                Console.WriteLine("\nEnter 1 for Import or 2 for Scan: ");
                importOrScanInput = ConsoleInput.PositiveInteger();
            } while (importOrScanInput != 1 && importOrScanInput != 2);

            List<string> updateTitles = new List<string>();
            if (importOrScanInput == 1)
            {
                Console.WriteLine("\nYou have chosen to Import. Importing...");
                updateTitles = ImportUpdateTitles();
                return updateTitles;
            }
            else if (importOrScanInput == 2)
            {
                Console.WriteLine("\nYou have chosen to Scan. ");
                Console.WriteLine("\nWould you like to check for available updates or installed updates?");
                //ConsoleInput c = new ConsoleInput();
                int installedOrAvailableinput = 0;
                do
                {
                    Console.WriteLine("\nEnter 1 for Available Updates or 2 for Installed Updates: ");
                    installedOrAvailableinput = ConsoleInput.PositiveInteger();
                } while (installedOrAvailableinput != 1 && installedOrAvailableinput != 2);

                if (installedOrAvailableinput == 1)
                {
                    Console.WriteLine("\nYou have chosen Available Updates. Scanning for available updates...");
                    updateTitles = WindowsUpdate.GetPendingUpdateTitles(0);
                }
                else if (installedOrAvailableinput == 2)
                {
                    Console.WriteLine("\nYou have chosen Installed Updates. Scanning for installed updates...");
                    updateTitles = WindowsUpdate.GetPendingUpdateTitles(1);
                }

                return updateTitles;
            }
            else
            {
                Console.WriteLine("Something went wrong. Input equals: '" + importOrScanInput + "'");
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
            string executionPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).Replace("file:\\", "");
            //Console.WriteLine(executionPath);
            if (File.Exists(executionPath + "\\" + "portable.txt"))
            {
                Configuration.IsPortable = true;
                Configuration.ConfigurationFolderPath = executionPath;
                //Configuration.ConfigurationFilePath = Configuration.ConfigurationFolderPath + "\\" + "config.txt";
            }

            Configuration.SetDefaultConfiguration(); //sets default values

            Console.WriteLine("Attempting to import configuration file at " + Configuration.ConfigurationFilePath);

            if (!Directory.Exists(Configuration.ConfigurationFolderPath)) //Config file is missing
            {
                Directory.CreateDirectory(Configuration.ConfigurationFolderPath);
            }
            if (!File.Exists(Configuration.ConfigurationFilePath))
            {
                Console.WriteLine("Configuration file does not exist. Recreating with default settings.");

                Configuration.SetDefaultConfiguration(); //sets default values

                FileIO.ExportStringListToFile(Configuration.ConfigurationFilePath, Configuration.GetCurrentConfiguration());
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
                Configuration.SetNewConfiguration(configValues);
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

        private List<UpdateInfo> CollectUpdateDataForTable(List<string> updateTitles, DataTable table)
        {
            Console.WriteLine("Attempting to collect data for " + updateTitles.Count + " updates...");
            int x = 0;
            List<UpdateInfo> updates = new List<UpdateInfo>();
            foreach (string updateTitle in updateTitles) //For each update
            {
                Console.WriteLine("Collecting data for update " + (x + 1) + " of " + updateTitles.Count + ".");

                Console.WriteLine("Title is: " + updateTitle);

                //If data exists in CSV file
                if (QueryController.DoesUpdateTitleExistInTable(table, updateTitle) == true)
                {
                    Console.WriteLine("Update data already exists in table. Skipping...");
                    DataRow[] rows = QueryController.GetUpdateInfoFromTable(table, updateTitle);
                    OrderedDictionary dictionary = new OrderedDictionary();
                    dictionary.Add(rows[0]["downloadUrls"].ToString(), rows[0]["languages"].ToString());
                    UpdateInfo update = new UpdateInfo( 
                        rows[0]["id"].ToString(),
                        rows[0]["title"].ToString(),
                        rows[0]["product"].ToString(), 
                        rows[0]["classification"].ToString(),
                        Convert.ToDateTime(rows[0]["lastUpdated"]),
                        rows[0]["version"].ToString(),
                        rows[0]["size"].ToString(),
                        dictionary
                        );
                    updates.Add(update);
                }
                else //Data doesn't exist in CSV file, collect it and populate the table
                {
                    string kb = Parser.GetKbFromTitle(updateTitle);

                    if (kb.Length > 0)
                    {
                        HtmlDocument siteAsHtml = WebController.GetSiteAsHTML(Configuration.CATALOG_URL + kb);
                        List<UpdateInfo> kbUpdates = Parser.GetUpdateInfoFromHTML(table, siteAsHtml);
                        if (kbUpdates.Count == 0)
                        {
                            Console.WriteLine("Update not found in Update Catalog. Skipping...");
                        }
                        else
                        {
                            table.PopulateTableWithUpdates(kbUpdates, Configuration.TableFolderPath, Configuration.TableName);
                            updates.AddRange(kbUpdates);
                        }
                    }
                }
                x++;
            }
            return updates;
        }
        private void StartDownloadManager(List<UpdateInfo> updates, DataTable table)//(List<string> updateTitles, DataTable table)
        {
            List<string> relevantProducts = new List<string>();
            foreach (UpdateInfo update in updates)
            {
                string[] products = update.Product.Split(',');
                foreach (string product in products)
                {
                    if (!relevantProducts.Contains(update.Product.Trim()))
                    {
                        relevantProducts.Add(update.Product.Trim());
                    }
                }
            }
            List<string> productList = GetFilteredProductList(table, relevantProducts);
            bool isFinished = false;
            while (isFinished == false)
            {
                Console.WriteLine("\nProducts: ");
                foreach (string product in productList)
                {
                    Console.WriteLine("{0}. {1}", (productList.IndexOf(product) + 1), product);
                }
                Console.WriteLine("Select a product to enable or disable it from the download list. \n(Leave blank and press enter to begin downloading): ");
                int input = ConsoleInput.PositiveIntegerAllowEmpty();
                if (input >= 1 && input <= productList.Count)
                {
                    if (productList[input - 1].EndsWith(" - DISABLED"))
                    {
                        productList[input - 1] = productList[input - 1].Replace(" - DISABLED", "");
                    }
                    else
                    {
                        productList[input - 1] = productList[input - 1] + " - DISABLED";
                    }
                }
                else if (input == -1)
                {
                    isFinished = true;
                }
                else
                {
                    Console.WriteLine("\nInvalid input. Try again... ");
                }
            }
            List<string> filteredProductList = new List<string>();
            List<string> filteredLanguageList = new List<string>
            {
                "english",
                "all"
            };
            foreach (string product in productList)
            {
                if (!product.Contains(" - DISABLED"))
                {
                    filteredProductList.Add(product);
                }
            }

            DownloadManager d = new DownloadManager(filteredProductList, filteredLanguageList);

            Console.WriteLine("Populating download queue...");
            d.PopulateDownloadQueue(updates, table);
            Console.WriteLine("Queue loading complete...");
            Console.WriteLine("Initializing download sequence...");
            d.DownloadFilesFromQueue();
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

        public List<string> GetFilteredProductList(DataTable table, List<string> relevantProducts)
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
                    if (!productList.Contains(trimmedProduct) && relevantProducts.Contains(trimmedProduct))
                    {
                        productList.Add(trimmedProduct);
                    }
                }

            }

            return productList;
        }
    }
}
