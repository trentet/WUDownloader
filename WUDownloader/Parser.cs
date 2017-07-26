using System;
using System.Collections.Generic;

namespace WUDownloader
{
    class Parser
    {
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

        public static string[] parseHtmlRow(int columnCount, List<string> rowHTML, int rowIndex)
        {
            string[] rowData = new string[columnCount];
            int indexOffset = (rowIndex * 7);

            rowData[0] = (rowHTML[0 + indexOffset].Split('"', '"')[1]);//id [0]
                int pFrom1 = rowHTML[0 + indexOffset].IndexOf(";\">") + ";\">".Length;
                int pTo1 = rowHTML[0 + indexOffset].LastIndexOf("</A>");

            rowData[1] = (rowHTML[0 + indexOffset].Substring(pFrom1, pTo1 - pFrom1)); //title [1]

            rowData[2] = (rowHTML[1 + indexOffset]); //os [2]

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

        public static object[] parseConfigFile(List<string> lines)
        {
            string downloadPath = "";
            string importPath = "";
            string tablePath = "";
            bool xp = false;
            bool vista = false;
            bool seven = false;
            bool eight = false;
            bool eightOne = false;
            bool ten = false;
            bool server2003 = false;
            bool server2008 = false;
            bool server2012 = false;
            bool server2012R2 = false;
            bool server2016 = false;
            string downloadPathPrefix = "downloadPath=";
            string importPathPrefix = "importPath=";
            string tablePathPrefix = "tablePath=";
            string xpPrefix = "downloadForXP=";
            string vistaPrefix = "downloadForVista=";
            string sevenPrefix = "downloadForSeven=";
            string eightPrefix = "downloadForEight=";
            string eightOnePrefix = "downloadForEightOne=";
            string tenPrefix = "downloadForTen=";
            string server2003Prefix = "downloadForServer2003=";
            string server2008Prefix = "downloadForServer2008=";
            string server2012Prefix = "downloadForServer2012=";
            string server2012R2Prefix = "downloadForServer2012R2=";
            string server2016Prefix = "downloadForServer2016=";


            foreach (string line in lines)
            {
                if (line.StartsWith(downloadPathPrefix)) //downloadPath
                {
                    downloadPath = line.Remove(0, downloadPathPrefix.Length);
                    continue;
                }
                else if (line.StartsWith(importPathPrefix)) //importPath
                {
                    importPath = line.Remove(0, importPathPrefix.Length);
                    continue;
                }
                else if (line.StartsWith(tablePathPrefix)) //tablePath
                {
                    tablePath = line.Remove(0, tablePathPrefix.Length);
                    continue;
                }
                else if (line.StartsWith(xpPrefix)) //XP
                {
                    if (line.Remove(0, xpPrefix.Length).Equals("true", StringComparison.InvariantCultureIgnoreCase))
                    {
                        xp = true;
                        continue;
                    }
                }
                else if (line.StartsWith(vistaPrefix)) //Vista
                {
                    if (line.Remove(0, vistaPrefix.Length).Equals("true", StringComparison.InvariantCultureIgnoreCase))
                    {
                        vista = true;
                        continue;
                    }
                }
                else if (line.StartsWith(sevenPrefix)) //7
                {
                    if (line.Remove(0, sevenPrefix.Length).Equals("true", StringComparison.InvariantCultureIgnoreCase))
                    {
                        seven = true;
                        continue;
                    }
                }
                else if (line.StartsWith(eightPrefix)) //8
                {
                    if (line.Remove(0, eightPrefix.Length).Equals("true", StringComparison.InvariantCultureIgnoreCase))
                    {
                        eight = true;
                        continue;
                    }
                }
                else if (line.StartsWith(eightOnePrefix)) //8.1
                {
                    if (line.Remove(0, eightOnePrefix.Length).Equals("true", StringComparison.InvariantCultureIgnoreCase))
                    {
                        eightOne = true;
                        continue;
                    }
                }
                else if (line.StartsWith(tenPrefix)) //10
                {
                    if (line.Remove(0, tenPrefix.Length).Equals("true", StringComparison.InvariantCultureIgnoreCase))
                    {
                        ten = true;
                        continue;
                    }
                }
                else if (line.StartsWith(server2003Prefix)) //Server 2003
                {
                    if (line.Remove(0, server2003Prefix.Length).Equals("true", StringComparison.InvariantCultureIgnoreCase))
                    {
                        server2003 = true;
                        continue;
                    }
                }
                else if (line.StartsWith(server2008Prefix)) //Server 2008
                {
                    if (line.Remove(0, server2008Prefix.Length).Equals("true", StringComparison.InvariantCultureIgnoreCase))
                    {
                        server2008 = true;
                        continue;
                    }
                }
                else if (line.StartsWith(server2012Prefix)) //Server 2012
                {
                    if (line.Remove(0, server2012Prefix.Length).Equals("true", StringComparison.InvariantCultureIgnoreCase))
                    {
                        server2012 = true;
                        continue;
                    }
                }
                else if (line.StartsWith(server2012R2Prefix)) //Server 2012 R2
                {
                    if (line.Remove(0, server2012R2Prefix.Length).Equals("true", StringComparison.InvariantCultureIgnoreCase))
                    {
                        server2012R2 = true;
                        continue;
                    }
                }
                else if (line.StartsWith(server2016Prefix)) //Server 2016
                {
                    if (line.Remove(0, server2016Prefix.Length).Equals("true", StringComparison.InvariantCultureIgnoreCase))
                    {
                        server2016 = true;
                        continue;
                    }
                }
            }

            Object[] configurationValues = new Object[14];
            configurationValues[0] = downloadPath;
            configurationValues[1] = importPath;
            configurationValues[2] = tablePath;
            configurationValues[3] = xp;
            configurationValues[4] = vista;
            configurationValues[5] = seven;
            configurationValues[6] = eight;
            configurationValues[7] = eightOne;
            configurationValues[8] = ten;
            configurationValues[9] = server2003;
            configurationValues[10] = server2008;
            configurationValues[11] = server2012;
            configurationValues[12] = server2012R2;
            configurationValues[13] = server2016;

            return configurationValues;
        }
    }
}