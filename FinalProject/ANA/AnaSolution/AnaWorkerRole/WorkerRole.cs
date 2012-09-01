using System.Diagnostics;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure.ServiceRuntime;
using Ana.Contracts.Business;
using StructureMap;
using Microsoft.WindowsAzure;
using System;

namespace Ana.Worker
{
    public class WorkerRole : RoleEntryPoint
    {
        public override void Run()
        {
            INotificationManager notificationManager = ObjectFactory.GetInstance<INotificationManager>();

            // This is a sample worker implementation. Replace with your logic.
            Trace.WriteLine("$projectname$ entry point called", "Information");
            while (true)
            {
                Thread.Sleep(5000);
                try
                {
                    notificationManager.ProcessPendingNotifications();
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message, "Error");
                }
            }
            
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
                configSetter(RoleEnvironment.GetConfigurationSettingValue(configName)));


            IoC.DependencyController.ConfigureForWorker();

            return base.OnStart();
        }
    }
}
