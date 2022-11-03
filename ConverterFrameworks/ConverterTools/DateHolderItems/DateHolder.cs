using System;
using System.Collections.Generic;

namespace ConverterTools.DateHolderItems
{
    public class DateHolder:IFormattable
    {
        public DateHolderTree Tree { get; private set; }

        public DateHolder()
        {
            this.Tree = new DateHolderTree();
        }

        #region Date Holder Items Builder
        public DateHolder Empty()
        {
            if (this.Tree.Root != null && (this.Tree.Root.ItemType != DateHolderItemType.Separator || this.Tree.Root.Right != null))
            {
                this.Separator(", ");
            }
            DateHolderTree yearTree = new DateHolderTree();
            yearTree.InsertRoot(string.Empty, null, null, DateHolderItemType.Empty);
            this.Tree.ImportTreeToRightRoot(yearTree);
            return this;
        }

        public DateHolder Date(string day, string month, string year)
        {
            if (this.Tree.Root != null && (this.Tree.Root.ItemType != DateHolderItemType.Separator || this.Tree.Root.Right != null))
            {
                this.Separator(" - ");
            }

            DateHolder h = new DateHolder();
            h.Day(day).Separator(".").Month(month).Separator(".").Year(year);
            this.Complex(h);
            return this;
        }

        public DateHolder Complex(DateHolder holder)
        {
            if (this.Tree.Root != null && (this.Tree.Root.ItemType != DateHolderItemType.Separator || this.Tree.Root.Right != null))
            {
                this.Separator(", ");
            }

            this.Tree.ImportTreeToRightRoot(holder.Tree);
            return this;
        }
        public DateHolder Day(string value,string prefix = null, string postfix = null)
        {
            if (this.Tree.Root != null && (this.Tree.Root.ItemType != DateHolderItemType.Separator || this.Tree.Root.Right!=null))
            {
                this.Separator(" - ");
            }

            DateHolderTree dayTree = new DateHolderTree();
            dayTree.InsertRoot(value, prefix, postfix, DateHolderItemType.Day);
            this.Tree.ImportTreeToRightRoot(dayTree);

            return this;
        }
        public DateHolder Month(string value, string prefix=null, string postfix=null)
        {
            if (this.Tree.Root != null && (this.Tree.Root.ItemType != DateHolderItemType.Separator || this.Tree.Root.Right != null))
            {
                this.Separator(".");
            }

            DateHolderTree monthTree = new DateHolderTree();
            monthTree.InsertRoot(value, prefix, postfix, DateHolderItemType.Month);
            this.Tree.ImportTreeToRightRoot(monthTree);

            return this;
        }
        public DateHolder Year(string value, string prefix=null, string postfix=null)
        {
            if (this.Tree.Root != null && (this.Tree.Root.ItemType != DateHolderItemType.Separator || this.Tree.Root.Right != null))
            {
                this.Separator(".");
            }

            DateHolderTree yearTree = new DateHolderTree();
            yearTree.InsertRoot(value, prefix, postfix, DateHolderItemType.Year);
            this.Tree.ImportTreeToRightRoot(yearTree);

            return this;
        }
        public DateHolder Separator(string value)
        {
            if (this.Tree.Root == null)
            {
                throw new ArgumentException("Нельзя начать дату с разделителя");
            }

            if (this.Tree.Root.Right==null && this.Tree.Root.ItemType == DateHolderItemType.Separator)
            {
                this.Tree.UpdateRoot(value, this.Tree.Root.Prefix, this.Tree.Root.Postfix, this.Tree.Root.ItemType);
            }
            else
            {
                this.Tree.InsertRoot(value, string.Empty, string.Empty, DateHolderItemType.Separator);
            }

            return this;
        }
        public DateHolder Brackes(string prefix, string postfix)
        {
            if (this.Tree.Root == null)
            {
                throw new ArgumentException("Нельзя начать дату с обрамления");
            }

            if (this.Tree.Root.Right == null && this.Tree.Root.ItemType == DateHolderItemType.Separator)
            {
                throw new ArgumentException("Нельзя обрамлять разделитель");
            }
            this.Tree.UpdateRoot(this.Tree.Root.Value, prefix, postfix, this.Tree.Root.ItemType);
            return this;
        }

        #endregion

        /// <summary>
        /// Представляет <see cref="DateHolder"/>, используя выбранный формат
        /// </summary>
        /// <param name="format"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format) || formatProvider==null)
            {
                return this.ToString();
            }

            return string.Format(formatProvider, string.Concat("{0:",format,"}"), this);
        }

        public override string ToString()
        {
            if (this.Tree == null)
            {
                return string.Empty;
            }

            return this.Tree.ToString();
        }
    }
}
