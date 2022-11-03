using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConverterTools.Configurations;

namespace DataSourceProviders.Connectors
{

    /// <summary>
    /// Работает, если будут параметры конвертера MsSqlServer и MsSqlDatabase.
    /// </summary>
    public class MsSqlServerDataSourceProvider : DbConnectionDataSourceProvider
    {
        private string msSqlServerParamKey;
        private string msSqlDatabaseParamKey;


        public MsSqlServerDataSourceProvider(IConfiguration configuration, string connectionDictionaryParamKey, string msSqlServerParamKey, string msSqlDatabaseParamKey) : base(configuration, "System.Data.SqlClient", connectionDictionaryParamKey)
        {
            this.msSqlServerParamKey = msSqlServerParamKey;
            this.msSqlDatabaseParamKey = msSqlDatabaseParamKey;
        }

        /// <summary>
        /// Если есть параметры "MsSqlServer", "MsSqlDatabase", то подойдет этот конструктор.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="connectionDictionaryParamKey"></param>
        public MsSqlServerDataSourceProvider(IConfiguration configuration, string connectionDictionaryParamKey)
            : this(configuration, connectionDictionaryParamKey,"MsSqlServer", "MsSqlDatabase")
        {

        }

        /// <summary>
        /// Если есть параметр "Шаблон строки подключения MsSql", то подойдет этот конструктор
        /// </summary>
        /// <param name="configuration"></param>
        public MsSqlServerDataSourceProvider(IConfiguration configuration)
            : this(configuration, "Шаблон строки подключения MsSql", "MsSqlServer", "MsSqlDatabase")
        {

        }

        protected override void AdditionalFillConnectionStringParams()
        {
            Dictionary<string, object> connectionParams = (Dictionary<string, object>)Configuration[this.connectionDictionaryParamKey];
            connectionParams["Data Source"] = base.Configuration[msSqlServerParamKey];
            connectionParams["Initial Catalog"] = base.Configuration[msSqlDatabaseParamKey];
            //connectionParams["Integrated Security"] = true;
        }
    }
}
