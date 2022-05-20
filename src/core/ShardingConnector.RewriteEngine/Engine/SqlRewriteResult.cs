using System.Collections.Generic;
using System.Data.Common;

namespace ShardingConnector.RewriteEngine.Engine
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/13 17:30:49
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class SqlRewriteResult
    {
        public SqlRewriteResult(string sql, IDictionary<string,DbParameter> parameters)
        {
            Sql = sql;
            Parameters = parameters;
        }

        public string Sql { get; }
    
        public  IDictionary<string,DbParameter> Parameters { get; }
    }
}
