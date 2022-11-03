using System;
using System.Collections.Generic;
using System.Linq;

namespace ConverterTools.Keepers
{
    /// <summary>
    /// Хранитель объектов, связанных списком
    /// </summary>
    /// <remarks>Позволяет накапливать объекты в иерархической структуре ограниченного размера</remarks>
    public class LinkedTreeMemento<T>
    {
        private int? capacity;
        private LinkedTreeNode<T> root;
        private LinkedTreeNode<T> currentLinkedTreeNode;

        /// <summary>
        /// Построитель объекта
        /// </summary>
        /// <param name="capacity">Максимальный размер связного списка.</param>
        /// <remarks>Если не указывать, то список безразмерный (не рекомендуется).</remarks>
        public LinkedTreeMemento(int? capacity = null)
        {
            this.capacity = capacity;
            this.root = new LinkedTreeNode<T>
            {
                Parent = default(LinkedTreeNode<T>),
                Item = default(T),
                Nodes = new LinkedList<LinkedTreeNode<T>>()
            };
            this.currentLinkedTreeNode = this.root;
        }

        /// <summary>
        /// Добавляет дочерний элемент в текущий узел (контекст)
        /// </summary>
        /// <param name="item">Элемент, который должен стать на место дочернего</param>
        public void AppendChild(T item)
        {
            if (this.currentLinkedTreeNode.Nodes.Count >= capacity)
            {
                this.currentLinkedTreeNode.Nodes.RemoveFirst();
            }

            this.currentLinkedTreeNode.Nodes.AddLast(new LinkedTreeNode<T>
            {
                Parent = this.currentLinkedTreeNode,
                Item = item,
                Nodes = new LinkedList<LinkedTreeNode<T>>()
            });
        }

        /// <summary>
        /// Вставляет элемент в хранилище на заданный уровень
        /// </summary>
        /// <param name="item">Значение добавляемого узла</param>
        /// <param name="level">Уровень глубины дерева, на который добавляется узел</param>
        public void AppendNode(T item, int level)
        {
            this.currentLinkedTreeNode = this.root;
            int i = 0;
            while (i++ < level)
            {
                if (!this.currentLinkedTreeNode.Nodes.Any())
                {
                    this.currentLinkedTreeNode.Nodes.AddLast(new LinkedTreeNode<T>
                    {
                        Parent = this.currentLinkedTreeNode,
                        Item = default(T),
                        Nodes = new LinkedList<LinkedTreeNode<T>>()
                    });
                }

                this.currentLinkedTreeNode = this.currentLinkedTreeNode.Nodes.Last.Value;
            }

            this.AppendChild(item);
            this.currentLinkedTreeNode = this.currentLinkedTreeNode.Nodes.Last.Value;
        }

        /// <summary>
        /// Возвращает в соответствии с правилом перебора.
        /// </summary>
        /// <param name="linkedTreeNodeVisitor">Traversal. Алгоритм перебора дерева</param>
        /// <returns>Последовательность хранящихся элементов</returns>
        public IEnumerable<T> ToEnumerable(ILinkedTreeNodeVisitor linkedTreeNodeVisitor)
        {
            return this.root.Accept(linkedTreeNodeVisitor);
        }
    }
}
