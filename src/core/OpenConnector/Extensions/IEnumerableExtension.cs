using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenConnector.Extensions
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/13 17:20:07
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public static class IEnumerableExtension
    {
        public static bool IsEmpty<T>(this IEnumerable<T> source)
        {
            return source==null||!source.Any();
        }
        public static bool IsEmpty<T>(this ICollection<T> source)
        {
            return source==null||source.Count==0;
        }
        public static bool IsNotEmpty<T>(this IEnumerable<T> source)
        {
            return !source.IsEmpty();
        }

        public static TValue Get<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key)
            where TKey : class
            where TValue : class
        {
            if (!dic.ContainsKey(key))
                return null;
            return dic[key];
        }

        public static T Next<T>(this IEnumerator<T> enumerator)
        {
            if (!enumerator.MoveNext()) {
                throw new InvalidOperationException();
            } else {
                return enumerator.Current;
            }
        }
        /// <summary>
        /// 求集合的笛卡尔积
        /// </summary>
        public static IEnumerable<IEnumerable<T>> Cartesian<T>(this IEnumerable<IEnumerable<T>> sequences)
        {
            IEnumerable<IEnumerable<T>> tempProduct = new[] {Enumerable.Empty<T>()};
            return sequences.Aggregate(tempProduct,
                (accumulator, sequence) =>
                    from accseq in accumulator
                    from item in sequence
                    select accseq.Concat(new[] {item})
            );
        }
        /// <summary>
        /// 按size分区,每个区size个数目
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="elements"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static IEnumerable<List<TSource>> Partition<TSource>(this IEnumerable<TSource> elements,int size)
        {
            return elements.Select((o, i) => new { Element = o, Index = i / size })
                .GroupBy(o => o.Index).Select(o => o.Select(g => g.Element).ToList());
        }
    }
}
