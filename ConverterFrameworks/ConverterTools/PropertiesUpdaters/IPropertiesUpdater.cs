namespace ConverterTools.PropertiesUpdaters
{
    /// <summary>
    /// Обновляет свойства элемента по единому свойству
    /// </summary>
    public interface IPropertiesUpdater
    {
        T Update<T>(T item, System.Type propsType) where T : class;
        T Update<T>(T item, string[] props) where T : class;
    }
}
