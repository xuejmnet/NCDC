using System.Collections.Generic;
using System.Data.Common;
using OpenConnector.ShardingAdoNet;

namespace OpenConnector.RewriteEngine.Engine
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
        public SqlRewriteResult(string sql, ParameterContext parameterContext)
        {
            Sql = sql;
            ParameterContext = parameterContext;
        }

        public string Sql { get; }
    
        public ParameterContext ParameterContext { get; }
    }
}
