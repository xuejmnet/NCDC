using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.DataStructure.RangeStructure;

namespace ShardingConnector.ShardingApi.Api.Sharding.Standard
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 14:24:03
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class RangeShardingValue<T>:IShardingValue where T:IComparable
    {
        public RangeShardingValue(string logicTableName, string columnName, Range<T> valueRange)
        {
            LogicTableName = logicTableName;
            ColumnName = columnName;
            ValueRange = valueRange;
        }

        public string LogicTableName { get; }
        public string ColumnName { get; }
        public Range<T> ValueRange { get; }
    }
}
