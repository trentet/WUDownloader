using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WUApiLib;
using System.Management;
using System.Net;
using System.Net.Sockets;

namespace WUDownloader
{
    class WindowsUpdate
    {
        public static List<string> GetPendingUpdateTitles()
        {
            //UpdateSession uSession = new UpdateSession();
            Type t = Type.GetTypeFromProgID("Microsoft.Update.Session", "pc12345.student.neumont.edu");
            UpdateSession uSession = new UpdateSession(); ;//(UpdateSession)Activator.CreateInstance(t);
            IUpdateSearcher uSearcher = uSession.CreateUpdateSearcher();
            ISearchResult uResult = uSearcher.Search("IsInstalled=0 and Type = 'Software' and IsHidden=0");//"IsInstalled=1");// and Type = 'Software'");

            List <string> updateTitles = new List<string>();
            foreach (IUpdate update in uResult.Updates)
            {
                updateTitles.Add(update.Title);
            }
            return updateTitles;

            //UpdateDownloader downloader = uSession.CreateUpdateDownloader();
            //downloader.Updates = uResult.Updates;
            //downloader.Download();
        }

        public static List<string> GetPendingUpdateTitlesRemote()
        {
            Type t = Type.GetTypeFromProgID("Microsoft.Update.Session", "remotehostname");
            UpdateSession uSession = (UpdateSession)Activator.CreateInstance(t);
            IUpdateSearcher uSearcher = uSession.CreateUpdateSearcher();
            int count = uSearcher.GetTotalHistoryCount();
            IUpdateHistoryEntryCollection uHistory = uSearcher.QueryHistory(0, count);
            for (int i = 0; i < count; ++i)
            {
                Console.WriteLine(string.Format("Title: {0}\tSupportURL: {1}\tDate: {2}\tResult Code: {3}\tDescription: {4}\r\n", uHistory[i].Title, uHistory[i].SupportUrl, uHistory[i].Date, uHistory[i].ResultCode, uHistory[i].Description));

            }
            return null;
        }

        public string GetHostName(string ipAddress)
        {
            try
            {
                IPHostEntry entry = Dns.GetHostEntry(ipAddress);
                if (entry != null)
                {
                    return entry.HostName;
                }
            }
            catch (SocketException ex)
            {
                //unknown host or
                //not every IP has a name
                //log exception (manage it)
            }

            return null;
        }
    }
}
