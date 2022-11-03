using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConverterTools.BulkInserts
{
    /// <summary>
    /// Представляет сервис быстрой вставки данных
    /// </summary>
    public interface IBulkInsert
    {
        /// <summary>
        /// Вставляет последовательность данных в 
        /// </summary>
        /// <typeparam name="T">Тип данных</typeparam>
        /// <param name="command">Команда для отправи данных в БД</param>
        /// <param name="items">Последовательность, ктороую необходимо вставить</param>
        void Execute<T>(string command, IEnumerable<T> items) where T : class;
    }
}
