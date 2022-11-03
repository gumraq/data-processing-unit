namespace ConverterTools
{
    /// <summary>
    /// Провайдер раскладки
    /// </summary>
    /// <typeparam name="T">Тип исходного объекта</typeparam>
    /// <typeparam name="THolder">Тип извлекаемого объекта</typeparam>
    public interface IParserProvider<in T, out THolder>
    {
        /// <summary>
        /// Извлекает объект типа <see cref="THolder"/> из объекта <see cref="T"/>
        /// </summary>
        /// <param name="record">Анализируемый объект</param>
        /// <returns>Результат раскладки</returns>
        THolder Parse(T record);
    }
}
