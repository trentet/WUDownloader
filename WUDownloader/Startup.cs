using System;

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
