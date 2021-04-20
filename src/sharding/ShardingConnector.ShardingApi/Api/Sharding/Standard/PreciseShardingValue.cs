using System;
using System.Collections.Generic;
using System.Text;

namespace ShardingConnector.ShardingApi.Api.Sharding.Standard
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 14:26:58
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class PreciseShardingValue<T>:IShardingValue where T:IComparable
    {
        public PreciseShardingValue(string logicTableName, string columnName, T value)
        {
            LogicTableName = logicTableName;
            ColumnName = columnName;
            Value = value;
        }

        public string LogicTableName { get; }
        public string ColumnName { get; set; }
        public T Value { get; }
    }
}
