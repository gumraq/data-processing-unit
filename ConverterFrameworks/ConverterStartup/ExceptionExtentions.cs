using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helpers.Extentions
{
    public static class ExceptionExtentions
    {
        /// <summary>
        /// Формирует набор строк из Exception и его всех вложенных в него InnerException
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static string AllMessage(this Exception exception)
        {
            if (exception == null)
                return null;

            var sb = new StringBuilder();
            
            Exception current = exception;
            do
            {
                sb.AppendLine(current.Message);
            } while ((current = current.InnerException) != null);

            return sb.ToString();
        }


        /// <summary>
        /// Формирует набор строк из Exception и его всех вложенных в него StackTrace
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static string AllStackTrace(this Exception exception)
        {
            if (exception == null)
                return null;

            var traces = new List<string>();
            
            Exception current = exception;
            do
            {
                traces.Add(current.StackTrace);
            } while ((current = current.InnerException) != null);

            traces.Reverse();

            return string.Join("\r\n", traces.ToArray());
        }


        /// <summary>
        /// Все Message и StackTrace исключения
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static string AllDetails(this Exception exception)
        {
           if (exception == null)
                return null;

            Exception current;

            var traces = new Stack<string>();
            current = exception;
            do
            {
                traces.Push(current.StackTrace);
                traces.Push(current.Message);
            } while ((current = current.InnerException) != null);
            traces.Push("StackTrace:");
            traces.Push(string.Empty);

            //соберём только Messages ещё раз в заголовке в нормальном порядке
            var headerMessages = new Queue<string>();
            current = exception;
            do
            {
                headerMessages.Enqueue(current.Message);
            } while ((current = current.InnerException) != null);

            return string.Join("\r\n", headerMessages.Concat(traces));
        }


        public static IEnumerable<Exception> AllInnerExceptions(this Exception exception)
        {
            if (exception == null)
                return null;

            return exception.SelfNDescendants(ex =>
            {
                AggregateException exAggr = ex as AggregateException;
                if (exAggr != null)
                {
                    return exAggr.InnerExceptions.Where(w=> w != null);
                }
                
                if (ex.InnerException != null)
                {
                    return new[] {ex.InnerException};
                }
                
                return null;
            });
        }

        private static IEnumerable<TNode> SelfNDescendants<TNode>(this TNode root, Func<TNode, IEnumerable<TNode>> childSelector)
        {
            if (root == null) throw new ArgumentNullException("root");

            var nodes = new Stack<TNode>(new[] { root });
            while (nodes.Any())
            {
                TNode node = nodes.Pop();
                yield return node;
                var childs = childSelector(node);
                if (childs != null)
                {
                    foreach (var n in childs) nodes.Push(n);
                }
            }
        }
    }
}