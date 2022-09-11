using System;
using System.Collections.Generic;
using System.Text;

namespace OpenConnector.Extensions
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 10:31:45
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public static class CollectionExtension
    {
        public static void AddIf<T>(this ICollection<T> source, bool @if, T target)
        {
            if (@if)
            {
                source.Add(target);
            }
        }
        public static void AddAll<T>(this ICollection<T> source,ICollection<T> target)
        {
            if (target.Count == 0)
            {
                return;
            }
            foreach (var t in target)
            {
                source.Add(t);
            }
        }
        public static void RemoveAll<T>(this ICollection<T> source,ICollection<T> target)
        {
            if (target.Count == 0)
            {
                return;
            }
            foreach (var t in target)
            {
                source.Remove(t);
            }
        }
    }
}
