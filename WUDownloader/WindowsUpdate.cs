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
            /*Note that, we have only found the updates available for this machine, but haven’t
            downloaded them yet.
            

            You can iterate the available updates to select only the required updates and add then to an
            UpdateCollection class which can be assigned to the below UpdateDownloader class so as to
            download them

            Now we have to create an UpdateDownloader class object to download the updates, as below
            */
            //UpdateDownloader downloader = uSession.CreateUpdateDownloader();
            //downloader.Updates = uResult.Updates;
            //downloader.Download();

        }
    }
}
