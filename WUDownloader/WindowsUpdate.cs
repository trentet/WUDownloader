using System;
using System.Collections.Generic;
using System.Net;
using WUApiLib;
using System.Linq;
using System.Runtime.InteropServices;

namespace WUDownloader
{
    class WindowsUpdate
    {
        public static List<string> GetPendingUpdateTitles(int isInstalled)
        {
            UpdateSession updateSession = new UpdateSession();
            try
            {
                Type stype = Type.GetTypeFromProgID("Microsoft.Update.Session");
                updateSession = (UpdateSession)Activator.CreateInstance(stype);
            }
            catch (COMException ce)
            {
                throw ce;
            }

            ISearchResult uResult =
                updateSession
                    .CreateUpdateSearcher()
                        .Search(
                            $"IsInstalled={isInstalled}" 
                            + " and " + 
                            "IsHidden=0"
                            //+ " and " +
                            //"Type='Software'"
                        );
                return (from IUpdate update in uResult.Updates
                        select update.Title).ToList();
            

            //UpdateDownloader downloader = uSession.CreateUpdateDownloader();
            //downloader.Updates = uResult.Updates;
            //downloader.Download();
        }

        /* Not yet implemented */
        //public static List<string> GetPendingUpdateTitlesRemote()
        //{
        //    //Does not work yet
        //    Type t = Type.GetTypeFromProgID("Microsoft.Update.Session", "remotehostname");
        //    UpdateSession uSession = (UpdateSession)Activator.CreateInstance(t);
        //    IUpdateSearcher uSearcher = uSession.CreateUpdateSearcher();
        //    int count = uSearcher.GetTotalHistoryCount();
        //    IUpdateHistoryEntryCollection uHistory = uSearcher.QueryHistory(0, count);
        //    for (int i = 0; i < count; ++i)
        //    {
        //        Console.WriteLine(string.Format("Title: {0}\tSupportURL: {1}\tDate: {2}\tResult Code: {3}\tDescription: {4}\r\n", uHistory[i].Title, uHistory[i].SupportUrl, uHistory[i].Date, uHistory[i].ResultCode, uHistory[i].Description));
        //    }
        //    return null;
        //}

        /* Not yet implemented */
        //public string GetHostName(string ipAddress)
        //{
        //    try
        //    {
        //        IPHostEntry entry = Dns.GetHostEntry(ipAddress);
        //        if (entry != null)
        //        {
        //            return entry.HostName;
        //        }
        //    }
        //    catch //(SocketException ex)
        //    {
        //        //unknown host or
        //        //not every IP has a name
        //        //log exception (manage it)
        //    }

        //    return null;
        //}
    }
}
