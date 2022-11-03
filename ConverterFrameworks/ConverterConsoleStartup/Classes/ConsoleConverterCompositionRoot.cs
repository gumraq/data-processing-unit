using Autofac;
using CompositionRoot;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Autofac.Core;
using LoggerModules;
using log4net;
using System.IO;

namespace ConverterConsoleStartup.Classes
{
    public class ConsoleConverterCompositionRoot:ConverterCompositionRoot
    {
        public ConsoleConverterCompositionRoot(string modulePath):base(modulePath)
        {
        }
        public ILog GetLogger()
        {
            this.container = this.container ?? this.Register();
            using (ILifetimeScope scope = container.BeginLifetimeScope())
            {
                if (!scope.IsRegistered<ILog>())
                {
                    throw new FileLoadException("Загруженная сборка не содержит конвертер", assembly.Location);
                }
                return scope.Resolve<ILog>();
            }
        }

        protected override IContainer Register()
        {
            ContainerBuilder builder = new ContainerBuilder();
            var types=assembly.GetTypes().Where(t => typeof(Module).IsAssignableFrom(t) && !t.Name.Contains("Log4Net"));
            foreach (var type in types)
            {
                builder.RegisterModule((Module)Activator.CreateInstance(type));
            }
            
            builder.RegisterModule(new ConsoleLog4NetModule());
            return builder.Build();
        }
    }
}
