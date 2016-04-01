using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Timers;

namespace OmnimarkAmazon.Service
{
    public partial class Service : ServiceBase
    {
        private static bool closeApplication = false;

        private Timer timer = null;

        private bool isBusy = false;

        public Service()
        {
            InitializeComponent();
            double interval = 6000;
            timer = new Timer(interval);
            timer.Elapsed += new ElapsedEventHandler(OnTick);
        }

        public void StartEngine()
        {
            try
            {
                if (!isBusy)
                {
                    isBusy = true;
                    Engine engine = new Engine();
                    engine.Process();
                    isBusy = false;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        protected override void OnStart(string[] args)
        {
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Start();
            
            Trace.WriteLine("Service ready...");
            Trace.Flush();
        }

        protected override void OnStop()
        {
            closeApplication = true;
            timer.AutoReset = false;
            timer.Enabled = false;

            Trace.WriteLine("Service Closed...");
            Trace.Flush();
        }

        protected virtual void OnTick(object sender, ElapsedEventArgs e)
        {
            StartEngine();
        }

        protected override void OnPause()
        {
            base.OnPause();
            closeApplication = true;
        }
       
        protected override void OnContinue()
        {
            base.OnContinue();
            closeApplication = false;
        }
    }
}
