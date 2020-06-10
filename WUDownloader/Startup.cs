using System;
using System.Reflection;
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
            Console.WriteLine($"Initializing WUDownloader v{Assembly.GetExecutingAssembly().GetName().Version.ToString()}"); ;
            Controller c = new Controller();
            c.Run();
        }
    }
}
