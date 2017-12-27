using System;
using System.Collections.Generic;
using System.Data;
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
        public static  List<DataRow> GetTypedDataRowsFromHTML(DataTable table, HtmlDocument siteAsHtml)
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
            Object[] cellData = new Object[table.Columns.Count];
            int numberOfColumnsNotParsedFromSite = 1;
            int numOfRows = unparsedRow.Count / (table.Columns.Count - numberOfColumnsNotParsedFromSite);
            
            List<DataRow> datarows = new List<DataRow>();
            for (int x = 0; x < numOfRows; x++)
            {
                cellData = ParseHtmlRow(cellData.Length, unparsedRow, x);
                DataRow datarow = TableBuilder.CreateDataRow(table, TableBuilder.AssignTypesToData(table, cellData));
                datarows.Add(datarow);
            }
            return datarows;
        }

        public static string[] ParseHeadersFromCsvStringList(List<string> csvAsStringList)
        {
            //Replace "," with "|" and then remove all quotes, then split by |
            string[] headers = csvAsStringList[0].Replace("\",\"", "\"|\"").Replace("\"", "").Split('|');
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

        public static string[] ParseHtmlRow(int columnCount, List<string> rowHTML, int rowIndex)
        {
            string[] rowData = new string[columnCount];
            int indexOffset = (rowIndex * 7);

            rowData[0] = (rowHTML[0 + indexOffset].Split('"', '"')[1]);//id [0]
                int pFrom1 = rowHTML[0 + indexOffset].IndexOf(";\">") + ";\">".Length;
                int pTo1 = rowHTML[0 + indexOffset].LastIndexOf("</A>");

            rowData[1] = (rowHTML[0 + indexOffset].Substring(pFrom1, pTo1 - pFrom1)); //title [1]
            rowData[2] = (rowHTML[1 + indexOffset]); //product [2]
            rowData[3] = (rowHTML[2 + indexOffset]); //classification [3]
            rowData[4] = (rowHTML[3 + indexOffset]); //lastUpdated [4]
            rowData[5] = (rowHTML[4 + indexOffset]); //version [5]
                int pFrom2 = rowHTML[5 + indexOffset].IndexOf("_size>") + "_size>".Length;
                int pTo2 = rowHTML[5 + indexOffset].LastIndexOf("</SPAN> <SPAN");

            rowData[6] = (rowHTML[5 + indexOffset].Substring(pFrom2, pTo2 - pFrom2)); //size [6]

            string downloadDialogSiteHTML = WebController.makePost(rowData[0], Configuration.Download_dialog_url);
            rowData[7] = string.Join(",", WebController.getDownloadURLs(downloadDialogSiteHTML)); //downloadUrls [7]

            return rowData;
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

            List<Object> configurationValues = new List<Object>();
            configurationValues.Add(rootPath);
            configurationValues.Add(downloadPath);
            configurationValues.Add(importPath);
            configurationValues.Add(tablePath);
            
            return configurationValues;
        }
    }
}