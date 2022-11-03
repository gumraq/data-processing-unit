using System.Collections.Generic;

namespace ConverterTools.ExtracterHolders
{
    public class AllBoolExtracterHolder<T> : ExtracterHolder<T, bool>
    {
        public AllBoolExtracterHolder(IEnumerable<IParserProvider<T, bool>> container) : base(container)
        {
        }

        public AllBoolExtracterHolder(IParserProvider<T, bool> parserProvider) : base(parserProvider)
        {
        }

        protected override bool Extract()
        {
            foreach (IParserProvider<T, bool> parser in base.Container)
            {
                if (!parser.Parse(base.Record))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
