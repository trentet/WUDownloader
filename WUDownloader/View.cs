using System;
using System.Collections.Generic;

namespace WUDownloader
{
    class View
    {
        public void PrintLines(List<string> lines)
        {
            // Display the file contents by using a foreach loop.
            foreach (string line in lines)
            {
                // Use a tab to indent each line of the file.
                Console.WriteLine("\t" + line);
            }
            // Keep the console window open in debug mode.
           // System.Console.ReadKey();
        }
    }
}
