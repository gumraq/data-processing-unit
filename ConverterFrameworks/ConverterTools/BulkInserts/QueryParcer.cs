using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConverterTools.BulkInserts
{
    public class QueryParcer
    {
        readonly Regex _reInsertInto;
        public QueryParcer()
        {
            _reInsertInto = new Regex(@"insert\s+into\s+(?<tbl>[^(]+)\s*[(](?<col1>.+)[)]\s*values\s*[(](?<col2>.+)[)]", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }

        public virtual string TableName(string queryText)
        {
            Match m = _reInsertInto.Match(queryText);
            if (m.Success)
            {
                var tmp = m.Groups["tbl"].Value.Trim(' ', '\n', '\r');

                if (tmp.Length > 2 & tmp[0] =='[' && tmp[tmp.Length - 1] == ']')
                {
                    if (!tmp.Contains("].["))
                        tmp = tmp.Substring(1, tmp.Length - 2);
                }
                    
                return tmp;
            }
            return string.Empty;
        }

        public virtual IEnumerable<string[]> ColumnsMapping(string queryText)
        {
            Match m = _reInsertInto.Match(queryText);
            if (m.Success)
            {
                string[] c1 = m.Groups["col1"].Value.Split(',').Select(s => s.Trim(' ', '\n', '\r').TrimStart('[').TrimEnd(']')).ToArray();
                string[] c2 = m.Groups["col2"].Value.Split(',').Select(s => s.Trim(' ', '\n', '\r').TrimStart('[').TrimEnd(']')).ToArray();
                for (int i = 0; i < c1.Length; i++)
                {
                    yield return new[] { c1[i], c2[i] };
                }
            }
        }
    }
}