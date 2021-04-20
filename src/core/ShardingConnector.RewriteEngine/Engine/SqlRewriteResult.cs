using System.Collections.Generic;

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
        public SqlRewriteResult(string sql, List<object> parameters)
        {
            Sql = sql;
            Parameters = parameters;
        }

        public string Sql { get; }
    
        public  List<object> Parameters { get; }
    }
}
