using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WUApiLib;
using System.Management;

namespace WUDownloader
{
    class WindowsUpdate
    {
        public static List<string> GetPendingUpdateTitles()
        {
            UpdateSession uSession = new UpdateSession();
            IUpdateSearcher uSearcher = uSession.CreateUpdateSearcher();
            ISearchResult uResult = uSearcher.Search("IsInstalled=0 and Type = 'Software'");

            List<string> updateTitles = new List<string>();
            foreach (IUpdate update in uResult.Updates)
            {
                updateTitles.Add(update.Title);
            }
            return updateTitles;

            //UpdateDownloader downloader = uSession.CreateUpdateDownloader();
            //downloader.Updates = uResult.Updates;
            //downloader.Download();
        }
    }
}
