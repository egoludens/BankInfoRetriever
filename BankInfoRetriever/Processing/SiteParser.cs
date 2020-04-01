using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace BankInfoRetriever
{
    class SiteParser
    {
        ILogger logger;
        public SiteParser(ILogger logger)
        {
            this.logger = logger;
        }

        public string GetDownoadableFileUrl(DateTime date)
        {
            string url = ConfigManager.FileAddressUrl.Replace("@DATE", date.ToString("dd.MM.yyyy"));

            logger.AddLogEntry(string.Format("Getting data from '{0}'...", url));
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                logger.AddLogEntry("Reading retrieved data...");
                Stream resStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(resStream);
                string resultingPage = reader.ReadToEnd();

                logger.AddLogEntry("Parsing page for file download address...");

                int indexOfBikForm = resultingPage.IndexOf(@"<a name=""BikFormData"">");
                int indexOfLink = indexOfBikForm == -1 ? -1 : resultingPage.IndexOf(@"<a href=", indexOfBikForm);
                int indexOfAddressStart = indexOfLink == -1 ? -1 : resultingPage.IndexOf(@"""", indexOfLink + 7);
                int indexOfAddressEnd = indexOfAddressStart == -1 ? -1 : resultingPage.IndexOf(@"""", indexOfAddressStart + 1);

                string fileAddress = indexOfAddressEnd == -1 ? "" : resultingPage.Substring(indexOfAddressStart + 1, indexOfAddressEnd - indexOfAddressStart - 1);
                if (fileAddress != "" && fileAddress[0] == '/')
                    fileAddress = response.ResponseUri.Scheme + "://" + response.ResponseUri.Host + fileAddress;

                return fileAddress;
            }
            catch (Exception ex)
            {
                logger.AddLogEntry(string.Format("ERROR: {0}", ex.Message));
                return null;
            }
        }

    }
}
