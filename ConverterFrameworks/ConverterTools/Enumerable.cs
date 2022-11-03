using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ConverterTools
{
    public static class Enumerable
    {
        /// <summary>
        /// Проецирует каждый элемент последовательности в новую форму.
        /// </summary>
        /// <typeparam name="TSource">Тип элементов source.</typeparam>
        /// <typeparam name="TResult">Тип значения, возвращаемый selector.</typeparam>
        /// <param name="source">Последовательность значений, для которых вызывается функция преобразования.</param>
        /// <param name="predicate"></param>
        /// <param name="selector">Функция преобразования, применяемая к каждому исходному элементу; 
        /// второй параметр функции представляет индекс исходного элемента;
        /// третий параметр представляет предыдущий элемент последовательности</param>
        /// <returns></returns>
        public static System.Collections.Generic.IEnumerable<TResult> Select<TSource, TResult>(
            this System.Collections.Generic.IEnumerable<TSource> source, System.Func<TSource, bool> predicate,
            System.Func<TSource, int, TResult> selector)
        {
            if (source == null)
            {
                throw new System.ArgumentNullException("source");
            }
            if (predicate == null)
            {
                throw new System.ArgumentNullException("predicate");
            }

            if (selector == null)
            {
                throw new System.ArgumentNullException("selector");
            }

            int i = -1;
            foreach (TSource item in source)
            {
                checked
                {
                    if (predicate(item))
                    {
                        i++;                        
                    }
                }
                yield return selector(item, i);
            }
        }

        /// <summary>
        /// Проецирует каждый элемент последовательности в новую форму.
        /// </summary>
        /// <typeparam name="TSource">Тип элементов source.</typeparam>
        /// <typeparam name="TResult">Тип значения, возвращаемый selector.</typeparam>
        /// <param name="source">Последовательность значений, для которых вызывается функция преобразования.</param>
        /// <param name="selector">Функция преобразования, применяемая к каждому исходному элементу; 
        /// второй параметр функции представляет индекс исходного элемента;
        /// третий параметр представляет предыдущий элемент последовательности</param>
        /// <returns></returns>
        public static System.Collections.Generic.IEnumerable<TResult> Select<TSource, TResult>(
            this System.Collections.Generic.IEnumerable<TSource> source,
            System.Func<TSource, int, TSource, TResult> selector)
        {
            if (source == null)
            {
                throw new System.ArgumentNullException("source");
            }

            if (selector == null)
            {
                throw new System.ArgumentNullException("selector");
            }

            int i = -1;
            TSource prevItem = default(TSource);
            foreach (TSource item in source)
            {
                checked
                {
                    i++;
                }
                yield return selector(item, i, prevItem);
                prevItem = item;
            }
        }

        /// <summary>
        /// Проецирует каждый элемент последовательности в новую форму.
        /// </summary>
        /// <typeparam name="TSource">Тип элементов source.</typeparam>
        /// <typeparam name="TResult">Тип значения, возвращаемый selector.</typeparam>
        /// <param name="source">Последовательность значений, для которых вызывается функция преобразования.</param>
        /// <param name="size"></param>
        /// <param name="selector">Функция преобразования, применяемая к каждому исходному элементу; 
        /// первый параметр функции представляет набор предыдущих элементов последовательности размером в size в том же порядке, что и в source;
        /// второй параметр функции представляет текущий элемент последовательности;
        /// второй параметр функции представляет следующий элемент последовательности;</param>
        /// <returns></returns>
        public static System.Collections.Generic.IEnumerable<TResult> Select<TSource, TResult>(this System.Collections.Generic.IEnumerable<TSource> source, int size, System.Func<Queue<TSource>, TSource, Queue<TSource>, TResult> selector)
        {
            if (source == null)
            {
                throw new System.ArgumentNullException("source");
            }

            if (selector == null)
            {
                throw new System.ArgumentNullException("selector");
            }
            Queue<TSource> prevQueue = new Queue<TSource>(size);
            Queue<TSource> nextQueue = new Queue<TSource>(size);

            int i = 0;
            foreach (TSource item in source)
            {
                nextQueue.Enqueue(item);
                if (i++ >= size)
                {
                    TSource it = nextQueue.Dequeue();
                    yield return selector(prevQueue, it, nextQueue);
                    prevQueue.Enqueue(it);
                    if (prevQueue.Count > size)
                    {
                        prevQueue.Dequeue();
                    }
                }
            }

            while (nextQueue.Any())
            {
                TSource it = nextQueue.Dequeue();
                yield return selector(prevQueue, it, nextQueue);
                prevQueue.Enqueue(it);
                if (prevQueue.Count > size)
                {
                    prevQueue.Dequeue();
                }
            }
        }


        /// <summary>
        /// Проецирует каждый элемент последовательности в новую форму.
        /// </summary>
        /// <typeparam name="TSource">Тип элементов source.</typeparam>
        /// <typeparam name="TResult">Тип значения, возвращаемый selector.</typeparam>
        /// <param name="source">Последовательность значений, для которых вызывается функция преобразования.</param>
        /// <param name="predicate">Функция преобразования, применяемая к каждому исходному элементу; 
        /// первый параметр функции представляет предыдущий элемент последовательности;
        /// второй параметр функции представляет текущий элемент последовательности;
        /// второй параметр функции представляет следующий элемент последовательности;</param>
        /// <returns></returns>
        public static System.Collections.Generic.IEnumerable<TSource> SkipWhile<TSource>(this System.Collections.Generic.IEnumerable<TSource> source, int size, System.Func<IEnumerable<TSource>, TSource, IEnumerable<TSource>, bool> predicate)
        {
            if (source == null)
            {
                throw new System.ArgumentNullException("source");
            }

            if (predicate == null)
            {
                throw new System.ArgumentNullException("predicate");
            }
            Queue<TSource> prevQueue = new Queue<TSource>(size);
            Queue<TSource> nextQueue = new Queue<TSource>(size);

            int i = 0;
            bool skip = true;
            foreach (TSource item in source)
            {
                nextQueue.Enqueue(item);
                if (i++ >= size)
                {
                    TSource it = nextQueue.Dequeue();
                    if (!predicate(prevQueue, it, nextQueue))
                    {
                        skip = false;
                    }
                    if (!skip)
                    {
                        yield return it;
                    }
                    
                    prevQueue.Enqueue(it);
                    if (prevQueue.Count > size)
                    {
                        prevQueue.Dequeue();
                    }
                }
            }

            while (nextQueue.Any())
            {
                TSource it = nextQueue.Dequeue();
                
                if (!predicate(prevQueue, it, nextQueue))
                {
                    skip = false;
                }
                if (!skip)
                {
                    yield return it;
                }
                prevQueue.Enqueue(it);
                if (prevQueue.Count > size)
                {
                    prevQueue.Dequeue();
                }
            }
        }

        public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source,
            System.Func<TSource, int, TResult> selector, System.Func<TSource, bool> predicate)
        {
            if (source == null)
            {
                throw new System.ArgumentNullException("source");
            }
            if (selector == null)
            {
                throw new System.ArgumentNullException("selector");
            }
            if (predicate == null)
            {
                throw new System.ArgumentNullException("predicate");
            }

            int index = -1;
            foreach (TSource element in source)
            {
                if (predicate(element))
                {
                    checked { index++; }
                }
                yield return selector(element, index);                    
            }
            //return SelectIterator<TSource, TResult>(source, selector);
        }


        /// <summary>
        /// Возвращает первый элемент последовательности, удовлетворяющий условию.
        /// </summary>
        /// <typeparam name="TSource">Тип элементов source.</typeparam>
        /// <param name="source">Последовательность значений, для которых вызывается функция преобразования.</param>
        /// <param name="predicate">логическая функция - предикат; 
        /// первый параметр функции представляет предыдущий элемент последовательности;
        /// второй параметр функции представляет текущий элемент последовательности;
        /// второй параметр функции представляет следующий элемент последовательности;</param>
        /// <returns></returns>
        public static TSource FirstOrDefault<TSource>(this System.Collections.Generic.IEnumerable<TSource> source, int size, System.Func<IEnumerable<TSource>, TSource, IEnumerable<TSource>, bool> predicate)
        {
            if (source == null)
            {
                throw new System.ArgumentNullException("source");
            }

            if (predicate == null)
            {
                throw new System.ArgumentNullException("predicate");
            }
            Queue<TSource> prevQueue = new Queue<TSource>(size);
            Queue<TSource> nextQueue = new Queue<TSource>(size);

            int i = 0;
            foreach (TSource item in source)
            {
                nextQueue.Enqueue(item);
                if (i++ >= size)
                {
                    TSource it = nextQueue.Dequeue();
                    if (predicate(prevQueue, it, nextQueue))
                    {
                        return it;
                    }
                    prevQueue.Enqueue(it);
                    if (prevQueue.Count > size)
                    {
                        prevQueue.Dequeue();
                    }
                }
            }

            while (nextQueue.Any())
            {
                TSource it = nextQueue.Dequeue();
                if (predicate(prevQueue, it, nextQueue))
                {
                    return it;
                }
                prevQueue.Enqueue(it);
                if (prevQueue.Count > size)
                {
                    prevQueue.Dequeue();
                }
            }

            return default(TSource);
        }

        /// <summary>
        /// Возвращает первый элемент последовательности, удовлетворяющий условию.
        /// </summary>
        /// <typeparam name="TSource">Тип элементов source.</typeparam>
        /// <param name="source">Последовательность значений, для которых вызывается функция преобразования.</param>
        /// <param name="predicate">логическая функция - предикат; 
        /// первый параметр функции представляет предыдущий элемент последовательности;
        /// второй параметр функции представляет текущий элемент последовательности;
        /// второй параметр функции представляет следующий элемент последовательности;</param>
        /// <returns></returns>
        public static IEnumerable<IGrouping<TSource, TSource>> GroupBy<TSource>(
            this System.Collections.Generic.IEnumerable<TSource> source,
            System.Func<TSource, bool> keyCondition,
            System.Func<TSource, bool> predicate)
        {
            if (source == null)
            {
                throw new System.ArgumentNullException("source");
            }

            if (predicate == null)
            {
                throw new System.ArgumentNullException("keySelector");
            }

            if (predicate == null)
            {
                throw new System.ArgumentNullException("predicate");
            }

            TSource key = default(TSource);
            List<TSource> group = new List<TSource>();
            foreach (TSource item in source)
            {
                if (predicate(item) && !EqualityComparer<TSource>.Default.Equals(key, default(TSource)))
                {
                    yield return new Grouping<TSource, TSource>(key, group);
                    group = new List<TSource>();
                    key = default(TSource);
                }

                group.Add(item);
                if (keyCondition(item))
                {
                    key = item;
                }
           }

        }

        /// <summary>
        /// Возвращает элемент последовательности, в качестве ключа, а все элементы идущие после него по следующего predicate = true в качестве группы
        /// </summary>
        /// <typeparam name="TSource">Тип элементов source.</typeparam>
        /// <param name="source">Последовательность значений, для которых вызывается функция преобразования.</param>
        /// <param name="predicate">логическая функция - предикат; 
        /// <returns></returns>
        public static IEnumerable<IGrouping<TSource, TSource>> GroupBy<TSource>(
            this System.Collections.Generic.IEnumerable<TSource> source,
            System.Func<TSource, bool> predicate)
        {
            if (source == null)
            {
                throw new System.ArgumentNullException("source");
            }

            if (predicate == null)
            {
                throw new System.ArgumentNullException("keySelector");
            }

            TSource key = default(TSource);
            List<TSource> group = new List<TSource>();
            bool start = true;
            foreach (TSource item in source)
            {
                if (predicate(item))
                {
                    if (!start)
                    {
                        yield return new Grouping<TSource, TSource>(key, group);
                        group = new List<TSource>();
                    }
                    key = item;
                }

                start = false;
                group.Add(item);
            }

            yield return new Grouping<TSource, TSource>(key, group);


        }

        /// <summary>
        /// Разбивает последовательность на сегменты.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="size">длина очереди предыдущих и последующих элементов</param>
        /// <param name="newSegmentPredicate">Функтор-предикат. Аргументы: очередь предыдущих элементов, текущий элемент, очередь следующих элементов</param>
        /// <returns></returns>
        public static System.Collections.Generic.IEnumerable<IEnumerable<TSource>> Segment<TSource>(this System.Collections.Generic.IEnumerable<TSource> source, int size, System.Func<Queue<TSource>, TSource, Queue<TSource>, bool> newSegmentPredicate)
        {
            if (source == null)
            {
                throw new System.ArgumentNullException("source");
            }

            if (newSegmentPredicate == null)
            {
                throw new System.ArgumentNullException("newSegmentPredicate");
            }

            Queue<TSource> prevQueue = new Queue<TSource>(size + 1);
            Queue<TSource> nextQueue = new Queue<TSource>(size + 1);
            List<TSource> segment = new List<TSource>();

            int i = 0;

            foreach (TSource item in source)
            {
                nextQueue.Enqueue(item);
                if (i++ >= size)
                {
                    TSource it = nextQueue.Dequeue();
                    bool isNewSegmentPredicate = newSegmentPredicate(prevQueue, it, nextQueue);
                    if (isNewSegmentPredicate && segment.Any())
                    {
                        yield return segment;
                        segment = new List<TSource>();
                    }
                    segment.Add(it);
                    prevQueue.Enqueue(it);
                    if (prevQueue.Count > size)
                    {
                        prevQueue.Dequeue();
                    }
                }
            }

            while (nextQueue.Any())
            {
                TSource it = nextQueue.Dequeue();
                bool isNewSegmentPredicate = newSegmentPredicate(prevQueue, it, nextQueue);
                if (isNewSegmentPredicate && segment.Any())
                {
                    yield return segment;
                    segment = new List<TSource>();
                }
                segment.Add(it);

                prevQueue.Enqueue(it);
                if (prevQueue.Count > size)
                {
                    prevQueue.Dequeue();
                }
            }

            yield return segment;
        }

        internal class Grouping<TKey, TElement> : IGrouping<TKey, TElement>
        {
            private readonly IEnumerable<TElement> elements;
            public Grouping(TKey key, IEnumerable<TElement> elements)
            {
                this.Key = key;
                this.elements = elements;
            }
            public Grouping(IEnumerable<TElement> elements): this(default(TKey), elements)
            {
            }
            public IEnumerator<TElement> GetEnumerator()
            {
                return elements.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public TKey Key { get; private set; }
        }
    }
}
