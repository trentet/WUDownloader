using System;
using System.Runtime.InteropServices;
using TLogger.Writers;

namespace WUDownloader
{
    class Startup
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Console.SetOut(new MultiTextWriter(Console.Out, new LogWriter(ref Configuration.Logger)));
            }
            catch (TypeInitializationException e)
            {
                Console.WriteLine(e.InnerException);
                throw;
            }
            Console.WriteLine("Initializing WUDownloader v1.1.0");
            Controller c = new Controller();
            c.Run();
        }
    }
}
