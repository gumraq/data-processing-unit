using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ConverterTools.Keepers
{
    public class Rubricator
    {
        private const int MaxLevel = 4;
        private string priznakField;
        private string valueField;
        private string levelField;
        private string idField;
        private int rubrCount;

        public Rubricator(string id,string priznakField, string valueField, string levelField)
        {
            this.priznakField = priznakField;
            this.valueField = valueField;
            this.levelField = levelField;
            this.idField = id;
        }

        public Rubricator(): this("Id","Priznak", "Rubrika", "Level")
        {
        }

        public string[] Rubrica { get; private set; }

        public void Push<T>(T item)
        {
            if (this.Rubrica==null)
            {
                this.Rubrica = new string[MaxLevel];
            }
            string priznak = this.GetPriznak<T>()(item);
            string rubrica = this.GetRubrica<T>()(item);
            string level = this.GetLevel<T>()(item);
            int id = this.GetId<T>()(item);

            if (!string.IsNullOrEmpty(rubrica))
            {
                int lvl;
                if (!int.TryParse(level, out lvl) || lvl <= 0 || lvl > rubrCount + 1)
                {
                    throw new Exception(string.Format("TableRow.Id = {0}.Ошибка в уровнях рубрик. Работа будет остановлена", id));
                }

                if (lvl <= rubrCount)
                {
                    for (int i = lvl - 1; i < MaxLevel; i++)
                    {
                        this.Rubrica[i] = string.Empty;
                    }
                    rubrCount= lvl-1;
                }

                this.Rubrica[rubrCount++] = rubrica;
            }
            else if (priznak == "1" && this.rubrCount > 0)
            {
                for (int i = 0; i < MaxLevel; i++)
                {
                    this.Rubrica[i] = string.Empty;
                }
                rubrCount = 0;
            }
        }

        public void Reset()
        {
            for (int i = 0; i < MaxLevel; i++)
            {
                this.Rubrica[i] = string.Empty;
            }
            rubrCount = 0;
        }

        protected virtual Func<T, string> GetPriznak<T>()
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "item");
            MemberExpression body = Expression.Property(param, this.priznakField);
            Expression<Func<T, string>> expr = Expression.Lambda<Func<T, string>>(body, param);
            return expr.Compile();
        }

        protected virtual Func<T, int> GetId<T>()
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "item");
            MemberExpression body = Expression.Property(param, this.idField);
            Expression<Func<T, int>> expr = Expression.Lambda<Func<T, int>>(body, param);
            return expr.Compile();
        }

        protected virtual Func<T, string> GetRubrica<T>()
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "item");
            MemberExpression body = Expression.Property(param, this.valueField);
            Expression<Func<T, string>> expr = Expression.Lambda<Func<T, string>>(body, param);
            return expr.Compile();
        }

        protected virtual Func<T, string> GetLevel<T>()
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "item");
            MemberExpression body = Expression.Property(param, this.levelField);
            Expression<Func<T, string>> expr = Expression.Lambda<Func<T, string>>(body, param);
            return expr.Compile();
        }


    }
}
