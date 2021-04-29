using System;
using System.Collections.Generic;
using System.Text;

namespace ShardingConnector.Extensions
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/29 16:28:05
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public static class StringExtension
    {
        public static string SubStringWithEndIndex(this string source,int startIndex,int endIndex,bool includeEndChar=false)
        {
            if (endIndex < startIndex)
                throw new InvalidOperationException($"end index:{endIndex} should ge start index:{startIndex}");
            int length = endIndex - startIndex;
            if (includeEndChar)
            {
                length++;
            }
            return source.Substring(startIndex, length);
        }
    }
}
