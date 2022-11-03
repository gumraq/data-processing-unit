namespace ConverterTools.ExtracterHolders
{
    using System.Collections.Generic;

    /// <summary>
    /// Извлечение объекта
    /// </summary>
    /// <typeparam name="T">Тип исходного объекта</typeparam>
    /// <typeparam name="THolder">Тип извлекаемого объекта</typeparam>
    public class ExtracterHolder<T,THolder>
    {
        /// <summary>
        /// Контейнер парсер-провайдеров
        /// </summary>
        /// <typeparam name="T">Тип исходного объекта</typeparam>
        /// <typeparam name="THolder">Тип извлекаемого объекта</typeparam>
        protected readonly IEnumerable<IParserProvider<T, THolder>> Container;

        public ExtracterHolder(IEnumerable<IParserProvider<T, THolder>> container)
        {
            Container = container;
        }

        /// <summary>
        /// Инициализирует <see><cref>ExtracterHolder</cref></see>с одним объектом <see><cref>IParserProvider</cref></see>
        /// </summary>
        /// <param name="parserProvider"></param>
        /// <typeparam name="T">Тип исходного объекта</typeparam>
        /// <typeparam name="THolder">Тип извлекаемого объекта</typeparam>
        public ExtracterHolder(IParserProvider<T, THolder> parserProvider) : this(System.Linq.Enumerable.Repeat(parserProvider, 1))
        {
        }

        /// <summary>
        /// Объект, из которого извлекаются данные
        /// </summary>
        /// <typeparam name="T">Тип исходного объекта</typeparam>
        protected T Record { get; set; }

        /// <summary>
        /// Извлекает объект, используя парсеры из коллекции
        /// </summary>
        /// <returns>Извлечённый объект</returns>
        /// <typeparam name="THolder">Тип извлекаемого объекта</typeparam>
        protected virtual THolder Extract()
        {
            foreach (IParserProvider<T, THolder> parser in Container)
            {
                THolder holder = parser.Parse(Record);
                if (!EqualityComparer<THolder>.Default.Equals(holder,default(THolder)))
                {
                    return holder;
                }
            }
            
            return default(THolder);
        }

        /// <summary>
        /// Извлекает <see cref="THolder"/> из исходного объекта
        /// </summary>
        /// <param name="record">Исходный объект</param>
        /// <returns>Извлечённый объект <see cref="THolder"/></returns>
        /// <typeparam name="THolder">Тип извлекаемого объекта</typeparam>
        public THolder Extract(T record)
        {
            Record = record;
            return Extract();
        }
    }
}