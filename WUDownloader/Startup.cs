using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WUDownloader
{
    class Startup
    {
        [STAThread]
        static void Main(string[] args)
        {
            Controller c = new Controller();
            c.Run();
        }
    }
}
