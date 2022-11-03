using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
//using DataSourceProviders;
//using Helpers.Retrier;

namespace ConverterTools.BulkInserts.MsSqlBulkInserts
{
    //public class MsSqlBulkInsert : IBulkInsert
    //{
    //    private readonly QueryParcer _queryParcer = new QueryParcer();
    //    private readonly IDataSourceProvider<string> _sourceProvider = null ;
    //    private readonly IDataSourceProvider<DbConnection> _sourceProvider2 = null;

    //    public MsSqlBulkInsert(string dbPath)
    //        : this(new SimpleDataSourceProvider(dbPath))
    //    {
    //    }

    //    public MsSqlBulkInsert(IDataSourceProvider<string> sourceProvider)
    //    {
    //        _sourceProvider = sourceProvider;
    //    }

    //    public MsSqlBulkInsert(IDataSourceProvider<DbConnection> sourceProvider)
    //    {
    //        _sourceProvider2 = sourceProvider;
    //    }

    //    public void Execute<T>(string command, IEnumerable<T> items) where T:class
    //    {
    //        if (command == null) throw new ArgumentNullException("command", "Не указана команда для вставки в БД");
    //        if (items == null) throw new ArgumentNullException("items", "Не переданы элементы для вставки в БД");

    //        string tableName = null;
    //        try
    //        {
    //            tableName = _queryParcer.TableName(command);
    //            string[][] columns = _queryParcer.ColumnsMapping(command).ToArray();
    //            Dictionary<string, string> filedsMapping = columns.ToDictionary(k => k[1], v => v[0]);

    //            string connection = _sourceProvider != null ? _sourceProvider.Source() 
    //                :_sourceProvider2.Source().ConnectionString;

    //            var esf = new EntitySafeProvider<T, int>(connection, 3* 60 * 60, new QueryRetrierFake(), new object() );
    //            esf.CreateAllBulkSafe(items, filedsMapping, tableName: tableName);
    //        }
    //        catch (Exception e)
    //        {
    //            throw new Exception(string.Format("Ошибка вставки в таблицу \"{0}\"",  tableName ?? "<null>" ), e);
    //        }
    //    }
    //}
}
