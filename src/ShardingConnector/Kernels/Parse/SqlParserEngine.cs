using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using ShardingConnector.Kernels.Parse.SqlExpression;
using ShardingConnector.Parser.Sql.Command;

namespace ShardingConnector.Kernels.Parse
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/7 8:22:10
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public class SqlParserEngine
    {
        private readonly string _databaseTypeName;
        public SqlParserEngine(string databaseTypeName)
        {
            _databaseTypeName = databaseTypeName;
        }
        public ISqlCommand Parse(string sql)
        {
            ISqlCommand result = Parse0(sql);
            return result;
        }

        private ISqlCommand Parse0(string sql)
        {
            return null;
        }
    }
}
