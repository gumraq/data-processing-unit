using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;

namespace ConverterTools.DateHolderItems
{
    public class DateHolderTree
    {
        public DateHolderNode Root { get; private set; }

        public void InsertRoot(string value, string prefix, string postfix, string itemType)
        {
            if (this.Root == null)
            {
                this.Root = new DateHolderNode(1, value, prefix, postfix, itemType);
            }
            else
            {
                DateHolderNode newNode = new DateHolderNode(this.MaxRight.Key + 1, value, prefix, postfix,
                    DateHolderItemType.Separator)
                {
                    Left = this.Root
                };
                this.Root.Parent = newNode;
                this.Root = newNode;
            }
        }

        /// <summary>
        /// Устанавливает префиксы для корня дерева
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="postfix"></param>
        public void UpdateRoot(string value, string prefix, string postfix, string itemType)
        {
            DateHolderNode newRoot = new DateHolderNode(this.Root.Key, value, prefix, postfix, itemType);
            newRoot.Left = this.Root.Left;
            newRoot.Right = this.Root.Right;
            this.Root = newRoot;
        }
        public void UpdateNodeByKey(int key, string value, string prefix, string postfix, string itemType)
        {
            DateHolderNode curr = this.Root;
            while (curr != null)
            {
                if (curr.Key < key)
                {
                    curr = curr.Right;
                }
                else if (curr.Key > key)
                {
                    curr = curr.Left;
                }
                else
                {
                    DateHolderNode newNode = new DateHolderNode(key, value, prefix, postfix, itemType)
                    {
                        Parent = curr.Parent,
                        Left = curr.Left,
                        Right = curr.Right
                    };

                    if (curr.Parent != null)
                    {
                        if (curr.Parent.Key > key)
                        {
                            curr.Parent.Left = newNode;
                        }
                        if (curr.Parent.Key < key)
                        {
                            curr.Parent.Right = newNode;
                        }
                    }
                    return;
                }
            }
        }

        public void ImportTreeToRightRoot(DateHolderTree node)
        {
            if (this.Root == null)
            {
                this.Root = node.Root;
                this.Root.Parent = null;
            }
            else
            {
                if (this.Root.Right != null)
                {
                    throw new ArgumentException("У узла уже есть правая ветка", "node");
                }

                this.Root.Right = this.CloneNode(this.Root, node.Root, this.Root.Key);
                this.Root.Right.Parent = this.Root;
            }
        }

        public DateHolderNode CloneNode(DateHolderNode parent, DateHolderNode treeNode, int keySeed)
        {
            if (treeNode == null)
            {
                return null;
            }

            DateHolderNode cloneNode = new DateHolderNode(treeNode.Key + keySeed, treeNode.Value, treeNode.Prefix,
                treeNode.Postfix, treeNode.ItemType);
            cloneNode.Left = this.CloneNode(cloneNode,treeNode.Left, keySeed);
            cloneNode.Right = this.CloneNode(cloneNode, treeNode.Right, keySeed);
            cloneNode.Parent = parent;
            return cloneNode;
        }

        /// <summary>
        /// Возвращает самый правй узел дерева
        /// </summary>
        private DateHolderNode MaxRight
        {
            get
            {
                DateHolderNode node = this.Root;
                if (this.Root == null)
                {
                    return null;
                }
                while (node.Right != null)
                {
                    node = node.Right;
                }
                return node;
            }
        }

        public override string ToString()
        {
            return this.Root.ToString();
        }


        public IEnumerable<DateHolderNode> NodesInorder()
        {
            return Recursive(this.Root);
        }

        public IEnumerable<DateHolderNode> NodesPreorder()
        {
            return this.RecursiveBack(this.Root);
        }

        private IEnumerable<DateHolderNode> Recursive(DateHolderNode node)
        {
            if (node.Left!=null)
            {
                foreach (DateHolderNode holderNode in this.Recursive(node.Left))
                {
                    yield return holderNode;
                }
            }
            yield return node;
            if (node.Right != null)
            {
                foreach (DateHolderNode holderNode in this.Recursive(node.Right))
                {
                    yield return holderNode;
                }
            }
        }


        private IEnumerable<DateHolderNode> RecursiveBack(DateHolderNode node)
        {
            if (node.Right != null)
            {
                foreach (DateHolderNode holderNode in this.RecursiveBack(node.Right))
                {
                    yield return holderNode;
                }
            }
            yield return node;
            if (node.Left != null)
            {
                foreach (DateHolderNode holderNode in this.RecursiveBack(node.Left))
                {
                    yield return holderNode;
                }
            }
        }


        //public IEnumerable<DateHolderNode> NodesInorder()
        //{
        //    if (this.Root == null)
        //    {
        //        yield break;
        //    }
            
        //    DateHolderNode pre;
        //    DateHolderNode current = this.Root;
        //    while (current != null)
        //    {
        //        if (current.Left == null)
        //        {
        //            Debug.Print(current.Key.ToString());
        //            yield return current;
        //            current = current.Right;
        //        }
        //        else
        //        {
        //            pre = current.Left;
        //            while (pre.Right != null && pre.Right != current)
        //            {
        //                pre = pre.Right;
        //            }

        //            if (pre.Right == null)
        //            {
        //                pre.Right = current;
        //                current = current.Left;
        //            } 
        //            else
        //            {
        //                pre.Right = null;
        //                Debug.Print(current.Key.ToString());
        //                yield return current;
        //                current = current.Right;
        //            }
        //        }
        //    }
        //}

        //public IEnumerable<DateHolderNode> NodesPreorder()
        //{
        //    if (this.Root == null)
        //    {
        //        yield break;
        //    }

        //    DateHolderNode pre;
        //    DateHolderNode current = this.Root;
        //    while (current != null)
        //    {
        //        if (current.Right == null)
        //        {
        //            yield return current;
        //            current = current.Left;
        //        }
        //        else
        //        {
        //            pre = current.Right;
        //            while (pre.Left != null && pre.Left != current)
        //            {
        //                pre = pre.Left;
        //            }

        //            if (pre.Left == null)
        //            {
        //                pre.Left = current;
        //                current = current.Right;
        //            }
        //            else
        //            {
        //                pre.Left = null;
        //                yield return current;
        //                current = current.Left;
        //            }
        //        }
        //    }
        //}
    }
}
