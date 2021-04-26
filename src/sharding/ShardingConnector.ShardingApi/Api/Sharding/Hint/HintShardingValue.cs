using System;
using System.Collections.Generic;
using System.Text;

namespace ShardingConnector.ShardingApi.Api.Sharding.Hint
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/26 11:35:25
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class HintShardingValue<T>:IShardingValue where T:IComparable
    {
        public HintShardingValue(string logicTableName, string columnName, ICollection<T> values)
        {
            this.LogicTableName = logicTableName;
            this.ColumnName = columnName;
            this.Values = values;
        }

        public  string LogicTableName { get; }

        public string ColumnName { get; }

        public ICollection<T> Values { get; }
    }
}
