using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConverterTools.Configurations;

namespace DataSourceProviders.Connectors
{
    public class AccessTechnoDataSourceProvider:DbConnectionDataSourceProvider
    {
        private string dbPathParamKey;
        public AccessTechnoDataSourceProvider(IConfiguration configuration, string connectionDictionaryParamKey, string dbPathParamKey)
            : base(configuration, "System.Data.OleDb", connectionDictionaryParamKey)
        {
            this.dbPathParamKey = dbPathParamKey;
        }

        public AccessTechnoDataSourceProvider(IConfiguration configuration, string dbPathParamKey)
            : this(configuration, "Шаблон строки подключения", dbPathParamKey)
        {
        }

        public AccessTechnoDataSourceProvider(IConfiguration configuration)
            : this(configuration,"Шаблон строки подключения", "TechnoDb")
        {
        }

        protected override void AdditionalFillConnectionStringParams()
        {
            Dictionary<string, object> connectionParams = (Dictionary<string, object>)Configuration[base.connectionDictionaryParamKey];
            object paramValue = base.Configuration[this.dbPathParamKey];
            if (paramValue is string)
            {
                connectionParams["Data Source"] = paramValue;
            }
            else if (paramValue is SerializableOpenFileDialog sofd)
            {
                connectionParams["Data Source"] = string.IsNullOrEmpty(sofd.FileName) ? (sofd.FileNames?.FirstOrDefault() ?? string.Empty) : sofd.FileName;
            }
            else
            {
                connectionParams["Data Source"] = string.Empty;
            }
            
        }
    }
}
