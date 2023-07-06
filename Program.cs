using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using MoscowCargo.Common;
using Quartz;
using Serilog;

namespace MoscowCargo.FysAgent
{
    class Program
    {
        public static async Task<int> Main(string[] args)
        {
            try
            {
                IHostBuilder hostBuilder = new HostBuilder()
                    .ConfigureAppConfiguration((hostContext, config) =>
                    {
                        config.SetBasePath(Directory.GetCurrentDirectory());
                        config.AddJsonFile("appsettings.json", optional: false);
                        Helper.Configuration = config.Build();
                    })
                    .ConfigureLogging((builderContext, logging) => logging.ClearProviders())
                    .UseSerilog((builderContext, configure) => configure.ReadFrom.Configuration(builderContext.Configuration))
                    .ConfigureServices((builderContext, services) =>
                    {
                        services.AddQuartz(Configure);
                        services.AddQuartzHostedService(options => { options.WaitForJobsToComplete = true; });
                        services.AddHostedService<AmqpService>();
                    });

                await hostBuilder.Build().RunAsync();
                return 0;
            }
            catch (Exception e)
            {
                return 1;
            }
        }

        private static void Configure(IServiceCollectionQuartzConfigurator configurator)
        {

            configurator.SchedulerId = "FysAgt";
            configurator.UseDefaultThreadPool(tp =>
            {
                tp.MaxConcurrency = 3;
            });
            configurator.UseMicrosoftDependencyInjectionJobFactory();
            configurator.SchedulerName = "SchedulerFysAgt";
            configurator.SetProperty("quartz.threadPool.threadCount", "3");
            configurator.SetProperty("quartz.scheduler.threadName", "FysAgtScheduler");
            configurator.SetProperty("quartz.scheduler.instanceName", "FysAgtQuartzScheduler");
            configurator.SetProperty("quartz.plugin.jobInitializer.fileNames", "quartz_jobs.xml");
            configurator.SetProperty("quartz.plugin.jobInitializer.type", "Quartz.Plugin.Xml.XMLSchedulingDataProcessorPlugin, Quartz.Plugins");
        }
    }
}