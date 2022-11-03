using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConverterTools.DateHolderItems;

namespace ConverterTools.Keepers
{
    /// <summary>
    /// Компонент иерархической структуры дат
    /// </summary>
    public class LinkedTreeNode<T>
    {
        /// <summary>
        /// Родитель
        /// </summary>
        public LinkedTreeNode<T> Parent { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        public T Item { get; set; }

        /// <summary>
        /// Связный список дочерних компонентов
        /// </summary>
        public LinkedList<LinkedTreeNode<T>> Nodes { get; set; }

        /// <summary>
        /// Возвращает уровень компонента в дереве
        /// </summary>
        public int? Level
        {
            get
            {
                int count = -1;
                LinkedTreeNode<T> linkedTreeNode = this.Parent;
                if (EqualityComparer<LinkedTreeNode<T>>.Default.Equals(linkedTreeNode, default(LinkedTreeNode<T>)) && EqualityComparer<T>.Default.Equals(this.Item, default(T)))
                {
                    return default(int?);
                }
                while (!EqualityComparer<LinkedTreeNode<T>>.Default.Equals(linkedTreeNode, default(LinkedTreeNode<T>)))
                {
                    linkedTreeNode = linkedTreeNode.Parent;
                    count++;
                }

                return count;
            }
        }

        /// <summary>
        /// Выполняет действие посетителя над компонентом
        /// </summary>
        /// <param name="linkedTreeNodeVisitor">Посетитель текущего компонента</param>
        /// <returns></returns>
        public IEnumerable<T> Accept(ILinkedTreeNodeVisitor linkedTreeNodeVisitor)
        {
            return linkedTreeNodeVisitor.Visit(this);
        }
    }
}
