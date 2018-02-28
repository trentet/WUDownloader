using System;

namespace WUDownloader
{
    class Startup
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Initializing WUDownloader v1.0.0");
            Controller c = new Controller();
            c.Run();
        }
    }
}
