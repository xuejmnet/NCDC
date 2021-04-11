using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
