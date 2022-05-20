using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

/*
* @Author: xjm
* @Description:
* @Date: DATE
* @Email: 326308290@qq.com
*/
namespace ShardingConnector.ShardingAdoNet
{
    public sealed class ParameterContext
    {
        public  IDictionary<string, DbParameter> Parameters { get; }
        public ParameterContext(List<DbParameter> dbParameters)
        {
            Parameters = dbParameters.ToDictionary(o => o.ParameterName, o => o);
        }
    }
}