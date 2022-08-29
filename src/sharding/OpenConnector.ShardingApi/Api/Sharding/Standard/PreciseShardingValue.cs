using System;
using System.Collections.Generic;
using System.Text;

namespace OpenConnector.ShardingApi.Api.Sharding.Standard
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 14:26:58
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class PreciseShardingValue: IShardingValue
    {

        public string LogicTableName { get; }
        public string ColumnName { get; set; }

        public IComparable Value { get; }

        public PreciseShardingValue(string logicTableName, string columnName, IComparable value) 
        {
            
            LogicTableName = logicTableName;
            ColumnName = columnName;
            Value = value;
        }
    }
}
