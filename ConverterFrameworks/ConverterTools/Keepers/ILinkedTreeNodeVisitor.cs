namespace ConverterTools.Keepers
{
    /// <summary>
    /// Посетитель компонента
    /// </summary>
    public interface ILinkedTreeNodeVisitor
    {
        /// <summary>
        /// Выполняет действия над компонентом дерева элементов
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="linkedTreeNode">Компонент дерева элементов</param>
        /// <returns>Последовательность элементов</returns>
        System.Collections.Generic.IEnumerable<T> Visit<T>(LinkedTreeNode<T> linkedTreeNode);
    }
}
