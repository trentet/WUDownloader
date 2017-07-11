using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace WUDownloader
{
    class WebController
    {
        public void openURLinNewTab(string CATALOG_URL, string kb)
        {
            Process.Start(CATALOG_URL + kb);
        }

        public static string makePost(string buttonID, string url)
        {
            // Create a request using a URL that can receive a post. 
            WebRequest request = WebRequest.Create(url);
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
        
        public static HtmlDocument getSiteAsHTML(string url)
        {
            //Initialize empty HtmlDocument
            HtmlDocument siteAsHTML = GetHtmlDocumentFromString("");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK) //If page requested successfully
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

                htmlString = readStream.ReadToEnd(); //Stores website's html as a string
                if (htmlString.Length > 0)
                {
                    //Converts html as a string into a HtmlDocument
                    siteAsHTML = GetHtmlDocumentFromString(htmlString);
                }
                else if (htmlString.Length == 0)
                {
                    Console.WriteLine("HTML not collected for url: " + url);
                }

                response.Close();
                readStream.Close();
            }
            else //If page requested unsuccessfully
            {
                Console.WriteLine("HTTPStatusCode Not OK -- HTML not collected for url: " + url);
            }
            return siteAsHTML;
        }

        public static List<string> getDownloadURLs(string downloadDialogSiteHTML)
        {
            //Splits string into an array by new line
            string[] result = downloadDialogSiteHTML.Split(new[] { '\r', '\n' });
            List<string> downloadURLs = new List<string>();
            foreach (string line in result)
            {
                if (line.StartsWith("downloadInformation[0].files[")) //All lines with a download URL start with this...
                {
                    if (line.Contains("].url = '")) //...but end with this before the URL. This allows for any index of file url
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

    private static System.Windows.Forms.HtmlDocument GetHtmlDocumentFromString(string html)
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
