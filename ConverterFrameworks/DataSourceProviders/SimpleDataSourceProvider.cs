using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConverterTools;
using ConverterTools.Configurations;

namespace DataSourceProviders
{
    public class SimpleDataSourceProvider : IDataSourceProvider<string>
    {
        private readonly string filePath;

        public SimpleDataSourceProvider(string filePath)
        {
            this.filePath = filePath;
        }

        public SimpleDataSourceProvider(IConfiguration configuration, string configKey = "OutDb")
        {
            if (configuration.Parameters.ContainsKey(configKey))
            {
                object paramValue = configuration[configKey];
                if (paramValue is string)
                {
                    this.filePath = (string) paramValue;
                }
                else if (paramValue is SerializableOpenFileDialog sofd)
                {
                    this.filePath = string.IsNullOrEmpty(sofd.FileName) ? (sofd.FileNames?.FirstOrDefault() ?? string.Empty) : sofd.FileName;
                }
                else
                {
                    this.filePath = string.Empty;
                }
            }
        }

        public string Source()
        {
            return this.filePath;
        }
    }
}
