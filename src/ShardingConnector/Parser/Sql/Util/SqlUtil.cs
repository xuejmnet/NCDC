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


        /**
         * Get exactly value for SQL expression.
         * 
         * <p>remove special char for SQL expression</p>
         * 
         * @param value SQL expression
         * @return exactly SQL expression
         */
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
