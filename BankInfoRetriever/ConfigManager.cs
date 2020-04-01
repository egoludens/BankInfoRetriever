using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankInfoRetriever
{
    public static class ConfigManager
    {
        public static string FileAddressUrl { get; private set; }
        public static string SqlConnectionString { get; private set; }

        internal static void ReadConfiguration()
        {
            FileAddressUrl = "http://cbr.ru/PSystem/payment_system/?UniDbQuery.Posted=True&UniDbQuery.To=@DATE#BikFormData"; // ConfigurationManager.AppSettings.Get("FileAddressUrl");
            SqlConnectionString = ConfigurationManager.ConnectionStrings["BankInfoRetriever.Properties.Settings.BankInfoConnectionString"].ConnectionString;
        }

    }
}
