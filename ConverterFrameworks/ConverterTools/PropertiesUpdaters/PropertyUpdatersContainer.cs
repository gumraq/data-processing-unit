using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConverterTools.PropertiesUpdaters
{
    /// <summary>
    /// Извлечение объекта
    /// </summary>
    public class PropertyUpdatersContainer
    {
        /// <summary>
        /// Контейнер парсер-провайдеров
        /// </summary>
        protected readonly IEnumerable<IPropertiesUpdater> Updaters;

        public PropertyUpdatersContainer(IEnumerable<IPropertiesUpdater> updaters)
        {
            this.Updaters = updaters;
        }

        /// <summary>
        /// Обновляет аргумент <see cref="T"/>
        /// </summary>
        /// <param name="record">Исходный объект</param>
        /// <param name="type">тип свойств, которые нужно обновить</param>
        public T Update<T>(T record, Type type) where T : class
        {
            if (!EqualityComparer<T>.Default.Equals(record, default(T)) && this.Updaters!=null)
            {
                foreach (IPropertiesUpdater updater in this.Updaters)
                {
                    updater.Update(record, type);
                }
            }
            return record;
        }

        /// <summary>
        /// Обновляет аргумент <see cref="T"/>
        /// </summary>
        /// <param name="record">Исходный объект</param>
        /// <param name="props">массив свойств, которые нужно обновить</param>
        public T Update<T>(T record, string[] props) where T : class
        {
            if (!EqualityComparer<T>.Default.Equals(record, default(T)) && this.Updaters != null)
            {
                foreach (IPropertiesUpdater updater in this.Updaters)
                {
                    updater.Update(record, props);
                }
            }
            return record;
        }
    }
}


