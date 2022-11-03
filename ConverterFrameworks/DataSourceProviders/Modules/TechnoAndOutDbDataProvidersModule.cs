using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using Autofac;
using ConverterTools;
using DataSourceProviders.Connectors;

namespace DataSourceProviders.Modules
{
    public class TechnoAndOutDbDataProvidersModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<AccessTechnoDataSourceProvider>().As<IDataSourceProvider<DbConnection>>().WithParameter("dbPathParamKey", "TechnoDb")
                .Named<IDataSourceProvider<DbConnection>>("TechnoConnector");
                
            // Закоментировать, если не нужно OleDb подключение к OutDB
            builder.RegisterType<AccessTechnoDataSourceProvider>().As<IDataSourceProvider<DbConnection>>().WithParameter("dbPathParamKey", "OutDb").Named<IDataSourceProvider<DbConnection>>("OutConnector");
            builder.RegisterType<SimpleDataSourceProvider>().As<IDataSourceProvider<string>>();
        }
    }
}
