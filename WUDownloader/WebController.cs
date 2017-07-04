using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WUDownloader
{
    class WebController
    {
        public void openURLinNewTab(string CATALOG_URL, string kb)
        {
            Process.Start(CATALOG_URL + kb);
        }

        public string makePost(string buttonID)//string url))
        {
            // Create a request using a URL that can receive a post. 
            WebRequest request = WebRequest.Create("https://www.catalog.update.microsoft.com/DownloadDialog.aspx");
            // Set the Method property of the request to POST.
            request.Method = "POST";
            // Create POST data and convert it to a byte array.
            string postData = "updateIDs=[{\"size\":0,\"languages\":\"\",\"uidInfo\":\"" + buttonID + "\",\"updateID\":\"" + buttonID + "\"}]&updateIDsBlockedForImport=&wsusApiPresent=&contentImport=&sku=&serverName=&ssl=&portNumber=&version=";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/x-www-form-urlencoded";
            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            if (!((HttpWebResponse)response).StatusDescription.ToString().Equals("OK"))
            {
                Console.WriteLine("Response Status: " + ((HttpWebResponse)response).StatusDescription);
            }
            // Get the stream containing content returned by the server.
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();

            // Display the content.
            //Console.WriteLine(responseFromServer);
            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();

            return responseFromServer;
        }
        
        public HtmlDocument getSiteAsHTML(string url)
        {
            HtmlDocument siteAsHTML = GetHtmlDocumentFromString("");
            //Console.WriteLine("Attempting to collect HTML for url: " + url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string htmlString;
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }

                htmlString = readStream.ReadToEnd();
                if (htmlString.Length > 0)
                {
                    siteAsHTML = GetHtmlDocumentFromString(htmlString);
                    //Console.WriteLine("HTML collected for url: " + url);
                }
                else if (htmlString.Length == 0)
                {
                    Console.WriteLine("HTML not collected for url: " + url);
                }

                response.Close();
                readStream.Close();
            }
            else
            {
                Console.WriteLine("HTTPStatusCode Not OK -- HTML not collected for url: " + url);
            }
            return siteAsHTML;
        }

        public List<string> getDownloadURLs(string id)
        {
            string downloadDialogSiteHTML = makePost(id);

            string[] result = downloadDialogSiteHTML.Split(new[] { '\r', '\n' });
            List<string> downloadURLs = new List<string>();
            foreach (string line in result)
            {
                if (line.StartsWith("downloadInformation[0].files[")) //0].url = '"))
                {
                    if (line.Contains("].url = '"))
                    {
                        int pFrom1 = line.IndexOf("].url = '") + "].url = '".Length;
                        int pTo1 = line.LastIndexOf("';");
                        string downloadURL = line.Substring(pFrom1, pTo1 - pFrom1); // Download URL
                        downloadURLs.Add(downloadURL);
                    }
                }
            }
            return downloadURLs;
        }

    public System.Windows.Forms.HtmlDocument GetHtmlDocumentFromString(string html)
        {
            WebBrowser browser = new WebBrowser();
            browser.ScriptErrorsSuppressed = true; //not necessesory you can remove it
            browser.DocumentText = html;
            browser.Document.OpenNew(true);
            browser.Document.Write(html);
            browser.Refresh();
            return browser.Document;
        }
    }
}
