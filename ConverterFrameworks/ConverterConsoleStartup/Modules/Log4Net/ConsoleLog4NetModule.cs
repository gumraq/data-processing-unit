using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Autofac;
using Autofac.Core;
using ConverterTools.Configurations;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Filter;
using log4net.Layout;
using log4net.Repository;
using Module = Autofac.Module;

namespace LoggerModules
{
    /// <summary>
    /// Модуль для инициализации логгера. Логгер ведет запись в специальный MessageListBox и в файл.
    /// В Файл пишутся Error, Warning и Info. В MessageListBox Warninng НЕ(!) пишутся.
    /// </summary>
    /// <remarks>Если завести IConfiguration со специальным параметром LogFileName, то имя файла
    /// будет не converter.log, а такое, как указано в парметре LogFileName</remarks>
    public class ConsoleLog4NetModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<PatternLayout>().As<ILayout>().WithParameter("pattern", "%date %-5level - %message%newline");
            builder.RegisterType<DenyAllFilter>()
                .Named<IFilter>("DenyAll");
            builder.RegisterType<LevelMatchFilter>()
                .WithProperty("AcceptOnMatch", true)
                .WithProperty("LevelToMatch", Level.Error)
                .Named<IFilter>("Errors");
            builder.RegisterType<LevelMatchFilter>()
                .WithProperty("AcceptOnMatch", true)
                .WithProperty("LevelToMatch", Level.Info)
                .Named<IFilter>("Infos");

            builder.RegisterType<RollingFileAppender>()
                .As<AppenderSkeleton>()
                .WithProperty("Name", "RollingFileAppender")
                .WithProperty("AppendToFile", true)
                .WithProperty("MaxSizeRollBackups", 5)
                .WithProperty("MaximumFileSize", "50mb")
                .WithProperty("RollingStyle", RollingFileAppender.RollingMode.Size)
                .WithProperty("StaticLogFileName", true)
                .OnActivating(h =>
                {
                    if (h.Context.IsRegistered<IConfiguration>())
                    {
                        IConfiguration configuration = h.Context.Resolve<IConfiguration>();
                        string logFileName = configuration.ParamValueOrDefault<string>("LogFileName") ?? "converter.log";
                        h.Instance.File = Path.Combine(GetAssemblyDirectory(), $"Log\\{logFileName}");
                    }
                    else
                    {
                        h.Instance.File = "CommonConverter.log";
                    }
                })
                .OnActivating(h =>
                {
                    h.Instance.Layout = h.Context.Resolve<ILayout>();
                    h.Instance.ActivateOptions();
                });
            builder.RegisterType<ConsoleAppender>().As<AppenderSkeleton>().OnActivating(h =>
            {
                foreach (IFilter filter in this.SomeFilter(h.Context))
                {
                    h.Instance.AddFilter(filter);
                }
                h.Instance.Layout = h.Context.Resolve<ILayout>();
                h.Instance.ActivateOptions();
            });

            builder.Register(c =>
            {
                var t = c.Resolve<IEnumerable<AppenderSkeleton>>().ToArray();
                var reps = LoggerManager.GetAllRepositories();
                ILoggerRepository loggerRepository = reps.FirstOrDefault(r => r.Name == "ConverterRepository") ?? LoggerManager.CreateRepository("ConverterRepository");
                BasicConfigurator.Configure(loggerRepository, t);
                return LogManager.GetLogger("ConverterRepository", "RollingFileLogger");
            }).As<ILog>().SingleInstance().OnRelease(f => f.Logger.Repository.Shutdown());
        }
        string GetAssemblyDirectory()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }

        IEnumerable<IFilter> SomeFilter(IComponentContext context)
        {
            yield return context.ResolveNamed<IFilter>("Errors");
            yield return context.ResolveNamed<IFilter>("Infos");
            yield return context.ResolveNamed<IFilter>("DenyAll");
        }
    }
}
