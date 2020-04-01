using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace BankInfoRetriever
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        static void Main(string[] args)
        {
            ConfigManager.ReadConfiguration();
            bool runWithGUI = args.Contains("/gui");
            if(runWithGUI)
            {
                (new MainForm()).ShowDialog();
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new BankInfoRetrievingService()
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
