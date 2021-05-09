﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShardingConnector.Extensions
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
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            return new HashSet<T>(source);
        }
        public static bool IsEmpty<T>(this IEnumerable<T> source)
        {
            return !source.Any();
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
    }
}