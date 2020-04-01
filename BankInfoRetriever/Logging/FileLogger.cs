using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankInfoRetriever
{
    class FileLogger : ILogger
    {
        public void AddLogEntry(string content)
        {
            File.AppendAllText(
                AppDomain.CurrentDomain.BaseDirectory + "Log.txt",
                string.Format("{0}: {1}\r\n", DateTime.Now.ToString(), content)
                );
        }
    }
}
