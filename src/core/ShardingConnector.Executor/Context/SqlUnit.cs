using System.Collections.Generic;
using System.Data.Common;

namespace ShardingConnector.Executor.Context
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 23 March 2021 21:30:29
* @Email: 326308290@qq.com
*/
    public sealed class SqlUnit
    {
        private readonly string _sql;
        private readonly IDictionary<string,DbParameter> _parameters;

        public SqlUnit(string sql,IDictionary<string,DbParameter> parameters)
        {
            _sql = sql;
            _parameters = parameters;
        }

        public string GetSql()
        {
            return _sql;
        }

        public IDictionary<string,DbParameter> GetParameters()
        {
            return _parameters;
        }
    }
}