using System;
using System.Collections.Generic;

namespace ShardingConnector.SqlParser
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
        private readonly List<object> _parameters;

        public SqlUnit(string sql,List<object> parameters)
        {
            _sql = sql;
            _parameters = parameters;
        }
    }
}