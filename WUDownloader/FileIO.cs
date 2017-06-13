using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WUDownloader
{
    class FileIO
    {
        public List<string> ImportFileToArray(string filepath)
        {
            List<string> lines = System.IO.File.ReadAllLines(filepath).ToList();
            return lines;
        }
    }
}
