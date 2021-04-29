using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.DataStructure.RangeStructure;

namespace ShardingConnector.ShardingApi.Api.Sharding.Complex
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 14:36:40
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ComplexKeysShardingValue<T>:IShardingValue where T:IComparable
    {
        public ComplexKeysShardingValue(string logicTableName, IDictionary<string, ICollection<T>> columnNameAndShardingValuesMap, IDictionary<string, Range<T>> columnNameAndRangeValuesMap)
        {
            LogicTableName = logicTableName;
            ColumnNameAndShardingValuesMap = columnNameAndShardingValuesMap;
            ColumnNameAndRangeValuesMap = columnNameAndRangeValuesMap;
        }

        public string LogicTableName { get; }
        public  IDictionary<string,ICollection<T>> ColumnNameAndShardingValuesMap { get; }
        public IDictionary<string,Range<T>> ColumnNameAndRangeValuesMap { get; }
    }
}
