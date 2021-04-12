using System;
using System.Collections.Generic;
using System.Text;

namespace ShardingConnector.Extensions
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
        public static void AddAll<T>(this ICollection<T> source,ICollection<T> target)
        {
            foreach (var t in target)
            {
                source.Add(t);
            }
        }
    }
}
