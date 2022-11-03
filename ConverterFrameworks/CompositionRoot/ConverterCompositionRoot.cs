using System;
using System.Collections.Generic;
using System.Configuration.Assemblies;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using Autofac;
using Autofac.Core;
using Autofac.Core.Lifetime;
using ConverterTools;
using ConverterTools.Configurations;


namespace CompositionRoot
{

    /// <summary>
    /// Построение корня композиции объектов для конвертирования
    /// </summary>
    public class ConverterCompositionRoot: IDisposable
    {
        /// <summary>
        /// Protected, чтобы можно в наследниках работать с Assembly
        /// </summary>
        protected Assembly assembly;
        protected IContainer container;

        public ConverterCompositionRoot(string modulePath)
        {
            this.assembly = Assembly.LoadFile(modulePath);
        }

        public ConverterCompositionRoot()
            : this("ConverterLibrary.dll")
        {
        }

        /// <summary>
        ///  Имя сборки-конвертера 
        /// </summary>
        public String ConverterName
        {
            get
            {
                return this.assembly.FullName;
            }
        }

        /// <summary>
        /// Версия сборки-конвертера
        /// </summary>
        public String ConverterVersion
        {
            get
            {
                return (FileVersionInfo.GetVersionInfo(assembly.Location)).FileVersion;
            }
        }

        /// <summary>
        /// Полный путь к загруженной сборки-конвертера
        /// </summary>
        public String BaseCode
        {
            get
            {
                return this.assembly.CodeBase;
            }
        }

        /// <summary>
        /// Заголовок сборки-конвертера
        /// </summary>
        public String ConverterTitle
        {
            get
            {
                return ((AssemblyTitleAttribute)assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0]).Title;
            }
        }

        /// <summary>
        /// Описание сборки-конвертера
        /// </summary>
        public String ConverterDescription
        {
            get
            {
                return ((AssemblyDescriptionAttribute)assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)[0]).Description;
            }
        }

        /// <summary>
        /// Регистрирует сборку конвертера и звгружает сведения о ней в корень композиции
        /// </summary>
        protected virtual IContainer Register()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(assembly);
            return builder.Build();
        }

        /// <summary>
        /// Строит конфигурацию конвертера
        /// </summary>
        /// <returns></returns>
        public IConfiguration CreateConfiguration()
        {
            this.container = this.container ?? this.Register();
            //using (IContainer container = this.Register())
            using (ILifetimeScope scope = container.BeginLifetimeScope())
            {
                if (!scope.IsRegistered<IConfiguration>())
                {
                    throw new FileLoadException("Загруженная сборка не содержит конвертер", assembly.Location);
                }
                return scope.Resolve<IConfiguration>();
            }
        }

        /// <summary>
        /// Строит конвертер
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IConverter> CreateConverter()
        {
            this.container = this.container ?? this.Register();
            //using (IContainer container = this.Register())
            using (ILifetimeScope scope = container.BeginLifetimeScope())
            {
                if (!scope.IsRegistered<IEnumerable<IConverter>>() && !scope.IsRegistered<IConverter>())
                {
                    throw new FileLoadException("Загруженная сборка не содержит конвертер", assembly.Location);
                }

                return scope.Resolve<IEnumerable<IConverter>>();
            }
        }

        public void Dispose()
        {
            this.container?.Dispose();
            this.container = null;
        }
    }
}