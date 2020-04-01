using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankInfoRetriever
{
    public interface ILogger
    {
        void AddLogEntry(string content);
    }
}
