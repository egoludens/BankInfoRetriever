using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace BankInfoRetriever
{
    class FileDownloader
    {
        ILogger logger;
        public FileDownloader(ILogger logger)
        {
            this.logger = logger;
        }

        public string DownloadFile(string fileUrl)
        {
            logger.AddLogEntry(string.Format("Downloading data from '{0}'...", fileUrl));
            try
            {
                string fileAddress = Path.GetTempFileName();
                using (var client = new WebClient())
                {
                    client.DownloadFile(fileUrl, fileAddress);
                }
                logger.AddLogEntry(string.Format("File saved to '{0}'...", fileAddress));
                return fileAddress;
            }
            catch(Exception ex)
            {
                logger.AddLogEntry(string.Format("ERROR: {0}", ex.Message));
                return null;
            }
        }

    }
}
