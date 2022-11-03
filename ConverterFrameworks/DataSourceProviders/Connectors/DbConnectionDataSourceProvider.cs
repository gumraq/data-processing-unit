using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ConverterTools;
using ConverterTools.Configurations;

namespace DataSourceProviders.Connectors
{
    public abstract class DbConnectionDataSourceProvider:IDataSourceProvider<DbConnection>
    {
        protected IConfiguration Configuration;
        protected string invariant;
        protected string connectionDictionaryParamKey;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration">Конфигурация конвертера</param>
        /// <param name="invariant">имя параметра конвертера с провайдером подключения</param>
        /// <param name="connectionDictionaryParamKey">имя параметра конвертера со свойствами подключения</param>
        public DbConnectionDataSourceProvider(IConfiguration configuration, string invariant, string connectionDictionaryParamKey)
        {
            this.Configuration = configuration;
            this.invariant = invariant;
            this.connectionDictionaryParamKey = connectionDictionaryParamKey;
        }

        protected abstract void AdditionalFillConnectionStringParams();


        public virtual DbConnection Source()
        {
            string dataProvider = this.invariant;
            Dictionary<string, object> connectionParams = (Dictionary<string, object>)Configuration[this.connectionDictionaryParamKey];
            this.AdditionalFillConnectionStringParams();

            if (string.IsNullOrEmpty(dataProvider))
            {
                throw new ArgumentNullException("DataProvider");
            }

            if (connectionParams == null || connectionParams.Count == 0)
            {
                throw new ArgumentNullException("ConnectionParams");
            }
#if STANDART_20
            // в STANDART_20 нет DbProviderFactories. Из-за этого приходится отдельно писать костыль.
            // Микрософт обещает в будущем вернуть этот класс, и тогда код будет таким же изящным, как раньше.
            DbProviderFactory factory = null;
            if (dataProvider?.ToLower() == "system.data.sqlclient")
            {
                factory = SqlClientFactory.Instance;
            }
            else if (dataProvider?.ToLower() == "system.data.oledb")
            {
                factory = OleDbFactory.Instance;
            }
            else
            {
                throw new Exception("Не предусмотренный провайдер данных");
            }
            
#else
            DbProviderFactory factory = DbProviderFactories.GetFactory(dataProvider);
#endif
            DbConnectionStringBuilder builder = factory.CreateConnectionStringBuilder();
            foreach (KeyValuePair<string, object> connectionParam in connectionParams)
            {
                builder.Add(connectionParam.Key, connectionParam.Value);
            }
            DbConnection connection = factory.CreateConnection();
            connection.ConnectionString = builder.ConnectionString;
            return connection;
        }
    }


}
