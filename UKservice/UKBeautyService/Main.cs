using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;

namespace OmnimarkAmazon.Service
{
    class Startup
    {
        public static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception ex = (Exception)args.ExceptionObject;
        }

        static void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                Startup s = new Startup();
                s.ConsoleCode();
            }
            else
            {
                ServiceBase[] servicesToRun;
                servicesToRun = new ServiceBase[]
                {
                    new Service()
                };

                ServiceBase.Run(servicesToRun);
            }

            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;
        }

        private void ConsoleCode()
        {
            #if DEBUG
            Engine engine = new Engine();
            while (true)
            {
                engine.Process();
            }
            #endif
        }
    }
}