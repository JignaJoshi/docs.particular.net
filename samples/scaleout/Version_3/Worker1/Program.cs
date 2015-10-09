﻿using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        #region Workerstartup
        Configure configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.Scaleout.Worker1");
        configure.DefaultBuilder();
        configure.EnlistWithDistributor();

        #endregion
        #region WorkerNameToUseWhileTestingCode
        //called after EnlistWithDistributor
        Address.InitializeLocalAddress("Samples.Scaleout.Worker1");
        #endregion
        configure.MsmqTransport();
        configure.InMemorySagaPersister();
        configure.InMemorySubscriptionStorage();
        configure.JsonSerializer();
        using (IStartableBus startableBus = configure.UnicastBus().CreateBus())
        {
            startableBus.Start(() => configure.ForInstallationOn<Windows>().Install());

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}