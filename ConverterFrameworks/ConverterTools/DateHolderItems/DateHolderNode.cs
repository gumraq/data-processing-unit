using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConverterTools.DateHolderItems
{
    public class DateHolderNode
    {
        public Int32 Key { get; private set; }
        public String Value { get; private set; }
        public String ItemType { get; private set; }
        public String Prefix { get; private set; }
        public String Postfix { get; private set; }
        public DateHolderNode Parent { get; set; }
        public DateHolderNode Left { get; set; }
        public DateHolderNode Right { get; set; }

        public DateHolderNode(int key, string value, string prefix, string postfix, string itemType)
        {
            this.Key = key;
            this.Value = value;
            this.ItemType = itemType;
            this.Prefix = prefix;
            this.Postfix = postfix;
        }

        public override string ToString()
        {
            return string.Concat(this.Prefix, this.Left == null ? string.Empty : this.Left.ToString(), this.Value,
                this.Right == null ? string.Empty : this.Right.ToString(), this.Postfix);
        }
    }
}
