using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace BankInfoRetriever.DB.Entities
{
    public abstract class DBEntityBase
    {
        public abstract void WriteToDatabase(SqlConnection sqlConnection);

        protected string GetStringFromDictionary(Dictionary<string, string> xModel, string key)
        {
            return GetStringFromDictionary(xModel, key, null);
        }

        protected string GetStringFromDictionary(Dictionary<string, string> xModel, string key, string defaultValue)
        {
            bool isFound = xModel.TryGetValue(key, out string result);
            if (!isFound)
            {
                if (defaultValue == null)
                {
                    throw new System.InvalidOperationException(string.Format("Key '{0}' not found.", key));
                }
                else
                {
                    return defaultValue;
                }
            }

            return result;
        }
    }
}
