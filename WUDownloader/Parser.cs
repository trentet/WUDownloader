using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WUDownloader
{
    class Parser
    {
        public List<string> ParseLinesContaining(List<string> lines, string searchParam)
        {
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

        public List<string> getRowDataFromHTML(List<string> rowHTML, int rowIndex)
        {
            List<string> rowData = new List<string>();
            int indexOffset = (rowIndex * 7);
            rowData.Add(rowHTML[0 + indexOffset].Split('"', '"')[1]);//id [0]
                int pFrom1 = rowHTML[0 + indexOffset].IndexOf(";\">") + ";\">".Length;
                int pTo1 = rowHTML[0 + indexOffset].LastIndexOf("</A>");
            rowData.Add(rowHTML[0 + indexOffset].Substring(pFrom1, pTo1 - pFrom1)); //title [1]
            rowData.Add(rowHTML[1 + indexOffset]); //os [2]
            rowData.Add(rowHTML[2 + indexOffset]); //classification [3]
            rowData.Add(rowHTML[3 + indexOffset]); //lastUpdated [4]
            rowData.Add(rowHTML[4 + indexOffset]); //version [5]
                int pFrom2 = rowHTML[5 + indexOffset].IndexOf("_size>") + "_size>".Length;
                int pTo2 = rowHTML[5 + indexOffset].LastIndexOf("</SPAN> <SPAN");
            rowData.Add(rowHTML[5 + indexOffset].Substring(pFrom2, pTo2 - pFrom2)); //size [6]
            rowData.Add(rowHTML[6 + indexOffset]); //buttonHTML [7]
           
            return rowData;
        }
    }
}