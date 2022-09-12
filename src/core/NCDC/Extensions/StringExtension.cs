using System;
using System.Collections.Generic;
using System.Text;

namespace NCDC.Extensions
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
        /// <summary>
        /// 替换第一个符合条件的字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="oldValue">所要替换掉的值</param>
        /// <param name="newValue">所要替换的值</param>
        /// <returns>返回替换后的值 所要替换掉的值为空或Null，返回原值</returns>
        public static string ReplaceFirst(this string value, string oldValue, string newValue)
        {
            if (string.IsNullOrEmpty(oldValue))
                return value;
 
            int idx = value.IndexOf(oldValue);
            if (idx == -1)
                return value;
            value = value.Remove(idx, oldValue.Length);
            return value.Insert(idx, newValue);
        }
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
