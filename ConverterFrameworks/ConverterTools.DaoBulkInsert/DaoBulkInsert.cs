using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
//using Helpers.Extentions;
using Microsoft.Office.Interop.Access.Dao;

namespace ConverterTools.BulkInserts
{
    public class DaoBulkInsert : IBulkInsert
    {
        private readonly QueryParcer _queryParcer = new QueryParcer();
        private readonly IDataSourceProvider<string> _sourceProvider;
        private readonly string dbPath;

        public DaoBulkInsert(string dbPath)
        {
            this.dbPath = dbPath;
        }

        public DaoBulkInsert(IDataSourceProvider<string> sourceProvider)
        {
            _sourceProvider = sourceProvider;
        }

        public void Execute<T>(string command, IEnumerable<T> items) where T : class
        {
            DBEngine dbEngine = null;
            Database db = null;

            string tableName = null;
            try
            {
                string filePath = _sourceProvider==null?this.dbPath:this._sourceProvider.Source();
                dbEngine = new DBEngine();
                db = dbEngine.OpenDatabase( filePath );

                tableName = _queryParcer.TableName( command );
                string[][] columns = _queryParcer.ColumnsMapping(command).ToArray();
                Recordset rs = db.OpenRecordset(tableName);

                KeyValuePair<Field, Func<T, object>>[] fields = new KeyValuePair<Field, Func<T, object>>[columns.Length];
                for (int i = 0; i < columns.Length; i++)
                {
                    var col = columns[i];
                    string columnName = col[0];
                    string propertyName = col[1];

                    Field rsField = GetRsField(rs, columnName);
                    Func<T, object> getter = GetGetter<T>(propertyName);

                    fields[i] = new KeyValuePair<Field, Func<T, object>>(rsField, getter);
                }
                    
                foreach (T item in items)
                {
                    try
                    {
                        rs.AddNew();

                        KeyValuePair<Field, Func<T, object>> currField;
                        for (var i = 0; i < fields.Length; i++)
                        {

                            currField = fields[i];
                            try
                            {
                                currField.Key.Value = currField.Value(item);
                            }
                            catch (Exception ex)
                            {
                                string columnName = columns[i][0];
                                string propertyName = columns[i][1];
                                string colValue = null;
                                try
                                {
                                    colValue = currField.Value(item).ToString();
                                }
                                catch { }
                                string message = string.Format("Ошибка при присвоении значения в колонку {0} из свойства {1}, значение \"{2}\"",
                                        columnName ?? "<null>",
                                        propertyName ?? "<null>",
                                        colValue ?? "<null>");
                                throw new Exception(message, ex);
                            }
                        }

                        rs.Update();
                    }
                    catch (Exception ex)
                    {
                        //попробуем дополнить описание ошибки описанием вставляемого объекта
                        string itemAsText = null;
                        try
                        {
                            var propertyNames = columns.Select(c => c[1]).ToArray();
                            itemAsText = ItemAsText(item, propertyNames);
                        }
                        catch { }
                        if (itemAsText != null)
                            throw new Exception(string.Format("Ошибка вставки объекта \"{0}\":{1}{2}", typeof(T).Name, Environment.NewLine, itemAsText), ex);
                        
                        throw;
                    }
                }

                rs.Close();
                db.Close();
                db = null;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Ошибка вставки в таблицу \"{0}\"", tableName ?? "<null>" ), e);
            }
            finally
            {
                if ( db != null )  
                    db.Close();

                if (dbEngine != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject( dbEngine );
                    dbEngine = null;
                }
            }
        }

        private static string ItemAsText<T>(T item, string[] propertyNames) where T : class
        {
            // TODO: Нужно переносить Helpers в Nuget Пакет. только тогда можно воспользоваться его инструментами.
            //DataTable dt = (new[] {item}).ToDataTable();
            //var ignoredProperties = dt.Columns.Cast<DataColumn>().Select(s => s.ColumnName).Where(w => !propertyNames.Contains(w)).ToArray();
            //foreach (var ignoredProperty in ignoredProperties)
            //{
            //    dt.Columns.Remove(dt.Columns[ignoredProperty]);
            //}
            string rowStr = null;// dt.Rows[0].AsString(everyPropOnNewLine: true);
            return rowStr;
        }

        private static Field GetRsField(Recordset rs, string columnName)
        {
            Field rsField;
            try
            {
                rsField = rs.Fields[columnName];
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Не найдена колонка \"{0}\"", columnName ?? "<null>"), ex);
            }

            if (rsField == null)
                throw new Exception(string.Format("Не найдена колонка \"{0}\"", columnName ?? "<null>"));

            return rsField;
        }

        /// <summary>
        /// Возвращает значение свойства объекта, приведённое к object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prop"></param>
        /// <returns></returns>
        protected virtual Func<T, object> GetGetter<T>(string prop)
        {
            try
            {
                ParameterExpression param = Expression.Parameter(typeof(T), "item");
                MemberExpression body = Expression.Property(param, prop);
                UnaryExpression body2 = Expression.Convert(body, typeof(object));
                Expression<Func<T, object>> expr = Expression.Lambda<Func<T, object>>(body2, param);
                return expr.Compile();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Не удалось создать Getter для свойства \"{0}.{1}\"",
                    typeof(T).FullName ?? "<null>", 
                    prop ?? "<null>"
                    ),ex);
            }
            
        }
    }
}
