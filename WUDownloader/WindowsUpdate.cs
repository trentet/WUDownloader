using System;
using System.Collections.Generic;
using Interop.WUApiLib;
using System.Net;
using System.Net.Sockets;

namespace WUDownloader
{
    class WindowsUpdate
    {
        public static List<string> GetPendingUpdateTitles(int isInstalled)
        {
            try
            {
                UpdateSession uSession = new UpdateSession();
                IUpdateSearcher uSearcher = uSession.CreateUpdateSearcher();
                ISearchResult uResult = uSearcher.Search("IsInstalled=" + isInstalled + " and IsHidden=0");// and Type = 'Software'

                List<string> updateTitles = new List<string>();
                foreach (IUpdate update in uResult.Updates)
                {
                    updateTitles.Add(update.Title);
                }
                return updateTitles;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                Console.WriteLine(e.StackTrace);
                throw;
            }
            

            //UpdateDownloader downloader = uSession.CreateUpdateDownloader();
            //downloader.Updates = uResult.Updates;
            //downloader.Download();
        }

        public static List<string> GetPendingUpdateTitlesRemote()
        {
            //Does not work yet
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
