using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

namespace BankInfoRetriever
{
    class FileManager
    {
        ILogger logger;
        public FileManager(ILogger logger)
        {
            this.logger = logger;
        }

        public void DeleteFile(string filePath)
        {
            logger.AddLogEntry(string.Format("Deleting file '{0}'...", filePath));
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    logger.AddLogEntry(string.Format("File '{0}' deleted.", filePath));
                }
            }
            catch (Exception ex)
            {
                logger.AddLogEntry(string.Format("ERROR: {0}", ex.Message));
            }
        }

        #region TempDirectory
        public string TempDirectory { get; private set; }

        public void CreateTempDirectory()
        {
            TempDirectory = Path.GetTempPath() + System.Guid.NewGuid().ToString();
            logger.AddLogEntry(string.Format("Creating temp directory to extract files: '{0}'...", TempDirectory));
            Directory.CreateDirectory(TempDirectory);
        }

        public void DeleteTempDirectory()
        {
            if (string.IsNullOrEmpty(TempDirectory))
                return;

            logger.AddLogEntry(string.Format("Deleting temp directory: '{0}'...", TempDirectory));
            try
            {
                if (Directory.Exists(TempDirectory))
                {
                    Directory.Delete(TempDirectory, true);
                    logger.AddLogEntry(string.Format("Directory '{0}' deleted.", TempDirectory));
                    TempDirectory = null;
                }
            }
            catch (Exception ex)
            {
                logger.AddLogEntry(string.Format("ERROR: {0}", ex.Message));
            }
        }
        #endregion

        public List<string> ExtractFilesToTempDirectory(string fileAddress)
        {
            List<string> extractedFiles = new List<string>();

            logger.AddLogEntry(string.Format("Extracting '{0}' to '{1}'...", fileAddress, TempDirectory));
            ZipFile.ExtractToDirectory(fileAddress, TempDirectory);

            logger.AddLogEntry("Reading zip file...");
            using (ZipArchive zipArchive = ZipFile.OpenRead(fileAddress))
            {
                foreach (ZipArchiveEntry zipArchiveEntry in zipArchive.Entries)
                {
                    logger.AddLogEntry(string.Format("Found file in zip archive: {0}", zipArchiveEntry.FullName));
                    extractedFiles.Add(TempDirectory + "\\" + zipArchiveEntry.FullName);
                }
            }
            return extractedFiles;
        }
    }
}
