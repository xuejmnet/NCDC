using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.Parser.Sql.Constant;

namespace ShardingConnector.Parser.Sql.Util
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/10 13:04:39
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public class SqlUtil
    {
        private SqlUtil()
        {
            
        }


        /// <summary>
        /// remove special char for SQL expression
        /// 获取移除了特殊字符后的值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetExactlyValue(string value)
        {
            return value?.Replace("[",string.Empty)
                .Replace("]", string.Empty)
                .Replace("`", string.Empty)
                .Replace("'", string.Empty)
                .Replace("\"", string.Empty);
        }
        /// <summary>
        /// 获取不带外圆括号的表达式
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetExpressionWithoutOutsideParentheses(string value)
        {
            int parenthesesOffset = GetParenthesesOffset(value);
            return 0 == parenthesesOffset ? value : value.Substring(parenthesesOffset, value.Length - parenthesesOffset);
        }
        private static int GetParenthesesOffset(string value)
        {
            int result = 0;
            while (Paren.Get(ParenEnum.PARENTHESES).GetLeftParen() == value[result])
            {
                result++;
            }
            return result;
        }
    }
}
