using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

namespace LoggerModules
{
    public class Log4NetCancellationFatalModule : Module
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
                        h.Instance.File = Path.Combine(Path.GetDirectoryName(configuration.ConfigFile), @"Log\converter.log");
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

            builder.RegisterType<MessageListBoxAppender>().As<AppenderSkeleton>().OnActivating(h =>
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
                ILoggerRepository loggerRepository = LoggerManager.CreateRepository("ConverterRepository");
                BasicConfigurator.Configure(loggerRepository, t);
                return LogManager.GetLogger("ConverterRepository", "RollingFileLogger");
            }).As<ILog>().SingleInstance();
        }
        IEnumerable<IFilter> SomeFilter(IComponentContext context)
        {
            yield return context.ResolveNamed<IFilter>("Errors");
            yield return context.ResolveNamed<IFilter>("Infos");
            yield return context.ResolveNamed<IFilter>("DenyAll");
        }
    }
}
