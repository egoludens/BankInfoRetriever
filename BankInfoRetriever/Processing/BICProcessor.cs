using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Data.SqlClient;
using System.Data;
using BankInfoRetriever.DB.Entities;

namespace BankInfoRetriever
{
    public class BICProcessor
    {
        ILogger logger;
        public BICProcessor(ILogger logger)
        {
            this.logger = logger;
        }

        public void Process(DateTime date)
        {
            logger.AddLogEntry(string.Format("Starting data retrieval for {0}...", date.ToString()));

            SiteParser siteParser = new SiteParser(logger);
            string fileUrl = siteParser.GetDownoadableFileUrl(date);
            if (string.IsNullOrEmpty(fileUrl))
                return;

            FileDownloader fileDownloader = new FileDownloader(logger);
            string filePath = fileDownloader.DownloadFile(fileUrl);
            if (string.IsNullOrEmpty(filePath))
                return;

            FileManager fileManager = new FileManager(logger);
            try
            {
                fileManager.CreateTempDirectory();
                List<string> extractedFiles = fileManager.ExtractFilesToTempDirectory(filePath);
                ProcessXmlFiles(extractedFiles);
            }
            catch (Exception ex)
            {
                logger.AddLogEntry(string.Format("ERROR: {0}", ex.Message));
            }
            finally
            {
                fileManager.DeleteTempDirectory();
                fileManager.DeleteFile(filePath);
            }
        }

        void ProcessXmlFiles(List<string> extractedFiles)
        {
            foreach (string filePath in extractedFiles)
            {
                if (filePath.ToLower().EndsWith(".xml"))
                {
                    ProcessXmlFile(filePath);
                }
                else
                {
                    logger.AddLogEntry(string.Format("File {0} ignored (only .xml files are processed)", filePath));
                }
            }
        }

        void ProcessXmlFile(string extractedFilePath)
        {
            logger.AddLogEntry(string.Format("Starting to process file '{0}'...", extractedFilePath));
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(extractedFilePath);

            logger.AddLogEntry("Connection to SQL database...");
            using (SqlConnection sqlConnection = new SqlConnection(ConfigManager.SqlConnectionString))
            {
                try
                {
                    sqlConnection.Open();
                    WriteXmlContentToDatabase(xDoc, sqlConnection);
                }
                catch(Exception ex)
                {
                    logger.AddLogEntry(string.Format("ERROR: {0}", ex.Message));
                }
            }
        }

        void WriteXmlContentToDatabase(XmlDocument xDoc, SqlConnection sqlConnection)
        {
            logger.AddLogEntry("Adding data to SQL database...");
            Dictionary<string, string> xBICDirectoryEntry = null;
            Dictionary<string, string> xParticipantInfo = null;
            Dictionary<string, string> xAccounts = null;

            DateTime updated = DateTime.Now.Date;

            int bicAdded = 0;
            int accountsAdded = 0;
            int participantInfoAdded = 0;

            foreach (XmlNode xNode in xDoc.DocumentElement.ChildNodes)
            {
                if (xNode.Name == "BICDirectoryEntry")
                {
                    xBICDirectoryEntry = CreateXModelFromNodeAttributes(xNode.Attributes);
                    DBBICDirectoryEntry dbBICDirectoryEntry = new DBBICDirectoryEntry(xBICDirectoryEntry, updated);
                    dbBICDirectoryEntry.WriteToDatabase(sqlConnection);
                    bicAdded++;
                    foreach(XmlNode xBICNode in xNode.ChildNodes)
                    {
                        if (xBICNode.Name == "ParticipantInfo")
                        {
                            xParticipantInfo = CreateXModelFromNodeAttributes(xBICNode.Attributes);
                            xParticipantInfo.Add("BIC", xBICDirectoryEntry["BIC"]);
                            DBParticipantInfo dbParticipantInfo = new DBParticipantInfo(xParticipantInfo);
                            dbParticipantInfo.WriteToDatabase(sqlConnection);
                            participantInfoAdded++;
                        }
                        else if (xBICNode.Name == "Accounts")
                        {
                            xAccounts = CreateXModelFromNodeAttributes(xBICNode.Attributes);
                            xAccounts.Add("BIC", xBICDirectoryEntry["BIC"]);
                            DBAccounts dbAccounts = new DBAccounts(xAccounts);
                            dbAccounts.WriteToDatabase(sqlConnection);
                            accountsAdded++;
                        }
                    }
                }
            }

            logger.AddLogEntry(string.Format("BIC Directory entries written: {0}", bicAdded));
            logger.AddLogEntry(string.Format("Participant Info entries written: {0}", participantInfoAdded));
            logger.AddLogEntry(string.Format("Accounts written: {0}", accountsAdded));
        }

        Dictionary<string, string> CreateXModelFromNodeAttributes(XmlAttributeCollection xAttributes)
        {
            var xModel = new Dictionary<string, string>();
            if (xAttributes != null)
                foreach (XmlAttribute xAttribute in xAttributes)
                    xModel.Add(xAttribute.Name, xAttribute.Value);

            return xModel;
        }
    }
}
