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
            if (!File.Exists(Configuration.ConfigurationFilePath)) //Config file is missing
            {
                Configuration.setDefaultConfiguration(); //sets default values

                FileIO.ExportStringArrayToFile(Configuration.ConfigurationFilePath, Configuration.getCurrentConfiguration());
            }
            else // Config File exists, import
            {
                List<string> configLines = FileIO.ImportFileToStringList(Configuration.ConfigurationFilePath);
                Object[] configValues = Parser.parseConfigFile(configLines);
                Configuration.setNewConfiguration(configValues);
            }
            

            //Import Update List from File
            List<string> lines = FileIO.ImportFileToStringList(Configuration.ImportPath + "\\Updates.txt"); 

            //Parse Update Titles from imported lines
            List<string> updateTitles = Parser.ParseLinesContaining(lines, "(KB");

            //Check if file exists
            if (File.Exists(Configuration.TablePath + ".csv")) //If exists
            {
                Console.WriteLine("CSV file exists. Importing...");
                //Import file
                List<string> csv = FileIO.ImportCsvToStringList(Configuration.TablePath + ".csv");

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
                FileIO.ExportDataTableToCSV(TableBuilder.Table, Configuration.TablePath);
            }

            Console.WriteLine("Attempting to collect data for " + updateTitles.Count + " updates...");
            int x = 0;
            foreach (string updateTitle in updateTitles) //For each update
            {
                Console.WriteLine("Collecting data for update " + (x+1) + " of " + updateTitles.Count + ".");

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
                    t.populateTableFromSite(siteAsHtml, Configuration.TablePath);
                }
                x++;
            }
            Console.WriteLine("Data collection complete.");

            DownloadManager d = new DownloadManager();
            d.setOsList();

            d.populateDownloadQueue(updateTitles);
            Console.WriteLine("Queue loading complete...");
            d.downloadFilesFromQueue(Configuration.DownloadPath);

            Console.WriteLine("Exiting...");
            System.Console.ReadKey();
        }
    }
}
