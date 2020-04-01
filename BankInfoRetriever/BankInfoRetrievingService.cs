using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace BankInfoRetriever
{
    public partial class BankInfoRetrievingService : ServiceBase
    {
        DateTime lastDayRun = new DateTime(1980, 1, 1); // Исходное значение - заглушка из прошлого.
        Timer timer;

        public BankInfoRetrievingService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            StartTimedProcessing();
        }

        void StartTimedProcessing()
        {
            timer = new Timer(5000); // Начнем обработку через 5 секунд после запуска сервиса, чтобы было немного времени на возможное обслуживание.
            timer.Elapsed += new ElapsedEventHandler(OnTimerElapsed);
            timer.Start();
        }

        void OnTimerElapsed(object source, ElapsedEventArgs e)
        {
            if (DateTime.Now.Date != lastDayRun)
            {
                lastDayRun = DateTime.Now.Date;
                timer.Stop();
                RunProcessing();
                timer.Interval = 3600000; // Раз в час проверяем, не пора ли загрузить свежие данные
                timer.Start();
            }
        }

        void RunProcessing()
        {
            (new BICProcessor(new FileLogger())).Process(DateTime.Now.Date);
        }

        protected override void OnStop()
        {
        }
    }
}
