using ConverterConsoleStartup.Classes;
using log4net;
using System;
using System.Data.Common;
using System.Data.OleDb;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

namespace ConverterConsoleStartup
{
    class Program
    {
       static AssemblyResolver _resolver;
       static ShellConfig _config;
        static void Main(string[] args)
        {
            _config = ShellConfig.Parse(args);
            Initialize();
            var converter = new Converter(_config.ConverterAssemblyPath, _config.ConverterConfigurationPath);
            converter.Execute();
        }

        static void Initialize()
        {
            _resolver = new AssemblyResolver(AppDomain.CurrentDomain, _config.ConverterAssemblyPath);
            DbProviderFactories.RegisterFactory("System.Data.OleDb", OleDbFactory.Instance);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

    }
}
