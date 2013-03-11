﻿using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace PosService
{
    static class Program
    {
        public const string EventLogName = "Pos Fikr-Malaysia";
        private static object _lockObject = new object();
        private static int _runningTasks = 0;

        public static void PrintToConsole()
        {
            lock (_lockObject)
            {
                _runningTasks += 1;
            }
            try
            {
                EventLog.WriteEntry(Program.EventLogName, "Starting work. Total active: " + _runningTasks, EventLogEntryType.Information,1);
                var r = new Random();
                System.Threading.Thread.Sleep(r.Next(1000));//3500
            }
            finally
            {
                lock (_lockObject)
                {
                    _runningTasks -= 1;
                }
            }
        }

        static void Main(string[] args)
        {
            if (System.Environment.UserInteractive)
            {

                if (args.Length > 0)
                {
                    switch (args[0])
                    {
                        case "i":
                            {
                                ServiceController ctl = ServiceController.GetServices().Where(s => s.ServiceName == Program.EventLogName).FirstOrDefault();
                                if (ctl != null)                                
                                {
                                    Console.WriteLine("Existing service detected, uninstalling...");
                                    ManagedInstallerClass.InstallHelper(new string[] { "/u", Assembly.GetExecutingAssembly().Location });
                                }
                                ManagedInstallerClass.InstallHelper(new string[] { Assembly.GetExecutingAssembly().Location });
                                
                                break;
                            }

                        case "u":
                            {
                                 ServiceController ctl = ServiceController.GetServices().Where(s => s.ServiceName == Program.EventLogName).FirstOrDefault();
                                 if (ctl != null)
                                 {
                                     Console.WriteLine("Existing service detected, uninstalling...");
                                     ManagedInstallerClass.InstallHelper(new string[] { "/u", Assembly.GetExecutingAssembly().Location });
                                 }
                                 else
                                 {
                                     Console.WriteLine("Service not detected");
                                 }
                                break;
                            }

                        case "c":
                            {
                                CoreService service = new CoreService();
                                service.DoStart();
                                Console.WriteLine("Press Enter to terminate ...");
                                Console.ReadLine();
                                service.DoStop();
                                break;
                            }
                    }
                }
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] { new CoreService() };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
