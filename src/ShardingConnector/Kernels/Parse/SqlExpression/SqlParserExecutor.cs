using System;
using System.Collections.Generic;
using System.Text;

namespace ShardingConnector.Kernels.Parse.SqlExpression
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/7 8:29:31
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class SqlParserExecutor
    {
        private readonly string _databaseTypeName;
        private readonly string _sql;

        public SqlParserExecutor(string databaseTypeName, string sql)
        {
            _databaseTypeName = databaseTypeName;
            _sql = sql;
        }
    }
}
