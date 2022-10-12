using System;
using System.Text.RegularExpressions;
using NCDC.CommandParser.Constant;
using NCDC.Extensions;

namespace NCDC.CommandParser.Util
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/10 13:04:39
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public static class SqlUtil
    {
        private const string SQL_END=";";
        private const string COMMENT_PREFIX = "/*";
        private const string COMMENT_SUFFIX = "*/";
        private const string EXCLUDED_CHARACTERS = "[]`'\"";
        private static readonly Regex SINGLE_CHARACTER_PATTERN = new Regex("^_|([^\\\\])_");
        private static readonly Regex SINGLE_CHARACTER_ESCAPE_PATTERN = new Regex("\\\\_");
        private static readonly Regex ANY_CHARACTER_PATTERN = new Regex("^%|([^\\\\])%");
        private static readonly Regex ANY_CHARACTER_ESCAPE_PATTERN = new Regex("\\\\%");
        
        

        public static decimal GetExactlyNumber(string value, int radix)
        {
            try
            {
                return Convert.ToInt64(value, radix);
            }
            catch (FormatException e)
            {
                return decimal.Parse(value);
            }
        }

        /// <summary>
        /// remove special char for SQL expression
        /// 获取移除了特殊字符后的值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string? GetExactlyValue(string? value)
        {
            if (value == null)
                return null;
            return value.Replace("[",string.Empty)
                .Replace("]", string.Empty)
                .Replace("`", string.Empty)
                .Replace("'", string.Empty)
                .Replace("\"", string.Empty);
        }
        public static string? GetExactlyValue(string? value,string reservedCharacters)
        {
            if (value == null)
                return null;
            var charArray = EXCLUDED_CHARACTERS.ToCharArray();
            charArray.RemoveAll(reservedCharacters.ToCharArray());
            var valueChars = value.ToCharArray();
            valueChars.RemoveAll(charArray);
            return new string(valueChars);
        }

        /// <summary>
        /// 移除空格
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetExactlyExpression(string value)
        {
            return value?.Replace(" ",string.Empty);
        }
        /// <summary>
        /// 获取不带外圆括号的表达式
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetExpressionWithoutOutsideParentheses(string value)
        {
            int parenthesesOffset = GetParenthesesOffset(value);
            return 0 == parenthesesOffset ? value : value.SubStringWithEndIndex(parenthesesOffset, value.Length - parenthesesOffset);
        }
        private static int GetParenthesesOffset(string value)
        {
            int result = 0;
            if (string.IsNullOrEmpty(value)) {
                return result;
            }
            while (Paren.Get(ParenEnum.PARENTHESES).GetLeftParen() == value[result])
            {
                result++;
            }
            return result;
        }


        /// <summary>
        /// 去掉注解
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static string TrimComment(string sql)
        {
            var result = sql;
            if (sql.StartsWith(COMMENT_PREFIX))
            {
                result = result.Substring(sql.IndexOf(COMMENT_PREFIX, StringComparison.Ordinal) + 2);
            }

            if (sql.EndsWith(SQL_END))
            {
                result = result.Substring(0, result.Length - 1);
            }

            return result.Trim();
        }
    }
}
