using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using ConverterTools;
using Microsoft.Office.Interop.Access.Dao;
using Enumerable = System.Linq.Enumerable;
using Access = Microsoft.Office.Interop.Access;

namespace ConverterTools.BulkInserts
{
    public class AccessLinkedTable
    {
        private IDataSourceProvider<DbConnection> fromDb;
        private IDataSourceProvider<string> mdbSourceProvider;

        /// <summary>
        /// Создает объект для прилинкования таблиц
        /// </summary>
        /// /// <param name="mdbSourceProvider">Путь к базе, в которой нужно создать прилинкованные таблицы</param>
        /// <param name="fromDb">Путь к базе, из которой нужно брать таблицы</param>
        public AccessLinkedTable(IDataSourceProvider<string> mdbSourceProvider, IDataSourceProvider<DbConnection> fromDb)
        {
            this.mdbSourceProvider = mdbSourceProvider;
            this.fromDb = fromDb;
        }

        public void Create(string tables, bool overwrite = true)
        {
            List<string[]> tableList = this.SplitSablrList(tables);
            IEnumerable<string> existTables;

            DBEngine dbEngine = null;
            Database db = null;
            using (DbConnection connection = this.fromDb.Source())
            {
                connection.Open();
                existTables = connection.GetSchema("Tables", new[] { null, null, null, "Table" }).Rows.Cast<DataRow>()
                    .Select(row => (string)row["TABLE_NAME"]).ToList();
            }

            try
            {
                dbEngine = new DBEngine();
                db = dbEngine.OpenDatabase(this.mdbSourceProvider.Source());
                foreach (string[] tableName in tableList.Any() ? tableList.GroupJoin(existTables, t => t[0], t => t, this.LinqResultSelector).Where(tn => tn != null) : existTables.Select(et => new[] { et, et }))
                {
                    TableDef table = db.TableDefs.Cast<TableDef>().FirstOrDefault(td => String.Equals(td.Name, tableName[1], StringComparison.CurrentCultureIgnoreCase));

                    if (!string.IsNullOrEmpty(table?.Connect))
                    {
                        db.TableDefs.Delete(tableName[1]);
                        table = null;
                    }

                    if (table == null)
                    {
                        table = db.CreateTableDef(tableName[1]);
                        table.Connect = $";DATABASE={this.fromDb.Source().DataSource}";
                        table.SourceTableName = tableName[0];
                        db.TableDefs.Append(table);
                    }
                }

                db.Close();
                db = null;
            }
            catch (Exception e)
            {
                string message = $"Ошибка при создании связанных таблиц: {e.Message}";
                throw new Exception(message, e);
            }
            finally
            {
                db?.Close();

                if (dbEngine != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(dbEngine);
                    dbEngine = null;
                }
            }
        }

        List<string[]> SplitSablrList(string tableList)
        {
            if (string.IsNullOrEmpty(tableList))
            {
                return new List<string[]>();
            }

            return tableList
                .Split(',')
                .Select(tn =>
                {
                    string normalTn = tn.Trim(' ', '.');
                    if (string.IsNullOrEmpty(normalTn))
                    {
                        return null;
                    }

                    string origName = string.Empty;
                    string aliasName = string.Empty;
                    int ind = normalTn.IndexOf(".");
                    if (ind < 0)
                    {
                        origName = normalTn;
                        aliasName = normalTn;
                    }
                    else
                    {
                        origName = normalTn.Substring(0, ind).Trim();
                        aliasName = normalTn.Substring(ind + 1).Trim();
                        if (string.IsNullOrEmpty(origName) && string.IsNullOrEmpty(aliasName))
                        {
                            return null;
                        }

                        if (string.IsNullOrEmpty(origName))
                        {
                            origName = aliasName;
                        }

                        if (string.IsNullOrEmpty(aliasName))
                        {
                            aliasName = origName;
                        }
                    }


                    return new[] { origName, aliasName };
                })
                .Where(names => names != null)
                .GroupBy(tn => tn[0]).Select(tng => new[] { tng.Key, tng.First()[1] })
                .ToList();
        }

        private string[] LinqResultSelector(string[] t, IEnumerable<string> ets)
        {
            if (!ets.Any())
            {
                throw new ArgumentException($"Таблица {t[0]} отсутсвует в базе данных");
            }

            return t;
        }
    }
}