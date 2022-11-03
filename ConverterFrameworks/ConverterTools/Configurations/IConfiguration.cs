using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConverterTools.Configurations
{
    /// <summary>
    /// Конфигурирование
    /// </summary>
    public interface IConfiguration
    {
        Dictionary<string, ParamDescWithValue> Parameters { get; }
        object this[string item] { get; set; }
        void Save();

        /// <summary>
        /// Возвращает значение параметра если он есть и его тип соответствует, иначе возвращает значение типа по умолчанию
        /// </summary>
        T ParamValueOrDefault<T>(string paramName, string propName = null);
        String ConfigFile { get; }
    }
}
