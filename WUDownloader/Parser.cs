using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using TableBuilderLibrary;

namespace WUDownloader
{
    public static class Parser
    {
        public static string GetKbFromTitle(string title)
        {
            string kb = "";
            if (title.Contains("(KB"))
            {
                kb = title.Split('(', ')')[1];
            }
            else
            {
                Console.WriteLine("Unable to find KB value in update title: " + title);
                Console.WriteLine("Update will be ignored.");
            }
            return kb;
        }
        public static  List<UpdateInfo> GetUpdateInfoFromHTML(DataTable table, HtmlDocument siteAsHtml)
        {
            //All elements with tag of "td"
            HtmlElementCollection cellElements = siteAsHtml.GetElementsByTagName("td");
            List<string> unparsedRow = new List<string>();
            foreach (HtmlElement elem in cellElements)
            {
                if (elem.GetAttribute("className").Contains("resultsbottomBorder") && !elem.GetAttribute("className").Contains("resultsIconWidth")) //excludes empty column
                {
                    string line = elem.InnerHtml;
                    unparsedRow.Add(line);
                }
            }

            int numberOfColumnsNotParsedFromSiteTable = 2;
            int numOfRows = unparsedRow.Count / (table.Columns.Count - numberOfColumnsNotParsedFromSiteTable);
            
            List<UpdateInfo> updates = new List<UpdateInfo>();
            for (int x = 0; x < numOfRows; x++)
            {
                UpdateInfo update = ParseHtmlRow(table.Columns.Count, unparsedRow, x);
                updates.Add(update);
            }
            return updates;
        }

        public static string[] ParseHeadersFromCsvStringList(List<string> csvAsStringList)
        {
            string[] headers = (string[])TableBuilder.SplitCSV(csvAsStringList[0]);
            return headers;
        }

        public static List<string> ParseLinesContaining(List<string> lines, string searchParam)
        {
            //Returns only lines containing the search parameter

            List<string> parsedLines = new List<string>();
            foreach (string line in lines)
            {
                if (line.Contains(searchParam))
                {
                    parsedLines.Add(line);
                }
            }
            return parsedLines;
        }

        public static UpdateInfo ParseHtmlRow(int columnCount, List<string> rowHTML, int rowIndex)
        {
            //string[] rowData = new string[columnCount];
            int indexOffset = (rowIndex * 7);

            string id = (rowHTML[0 + indexOffset].Split('"', '"')[1]);//id [0]
                int pFrom1 = rowHTML[0 + indexOffset].IndexOf(";\">") + ";\">".Length;
                int pTo1 = rowHTML[0 + indexOffset].LastIndexOf("</A>");

            string title = (rowHTML[0 + indexOffset].Substring(pFrom1, pTo1 - pFrom1)); //title [1]
            string product = (rowHTML[1 + indexOffset]); //product [2]
            string classification = (rowHTML[2 + indexOffset]); //classification [3]
            DateTime lastUpdated = Convert.ToDateTime(rowHTML[3 + indexOffset]); //lastUpdated [4]
            string version = (rowHTML[4 + indexOffset]); //version [5]
                int pFrom2 = rowHTML[5 + indexOffset].IndexOf("_size>") + "_size>".Length;
                int pTo2 = rowHTML[5 + indexOffset].LastIndexOf("</SPAN> <SPAN");

            string size = (rowHTML[5 + indexOffset].Substring(pFrom2, pTo2 - pFrom2)); //size [6]

            string downloadDialogSiteHTML = WebController.MakePost(id, Configuration.Download_dialog_url);
            OrderedDictionary downloadUrls = WebController.GetDownloadURLs(downloadDialogSiteHTML); //downloadUrls [7]

            UpdateInfo update = new UpdateInfo(id, title, product, classification, lastUpdated, version, size, downloadUrls);
            return update;
        }

        public static List<Object> ParseConfigFile(List<string> lines)
        {
            string rootPath = "";
            string downloadPath = "";
            string importPath = "";
            string tablePath = "";

            foreach (string line in lines)
            {
                if (line.StartsWith(Configuration.RootPathPrefix)) //downloadPath
                {
                    rootPath = line.Remove(0, Configuration.RootPathPrefix.Length);
                    continue;
                }
                else if (line.StartsWith(Configuration.DownloadPathPrefix)) //downloadPath
                {
                    downloadPath = line.Remove(0, Configuration.DownloadPathPrefix.Length);
                    continue;
                }
                else if (line.StartsWith(Configuration.ImportPathPrefix)) //importPath
                {
                    importPath = line.Remove(0, Configuration.ImportPathPrefix.Length);
                    continue;
                }
                else if (line.StartsWith(Configuration.TablePathPrefix)) //tablePath
                {
                    tablePath = line.Remove(0, Configuration.TablePathPrefix.Length);
                    continue;
                }
            }

            List<Object> configurationValues = new List<Object>
            {
                rootPath,
                downloadPath,
                importPath,
                tablePath
            };

            return configurationValues;
        }
    }
}