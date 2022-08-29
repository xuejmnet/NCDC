using System;
using OpenConnector.DataStructure.RangeStructure;

namespace OpenConnector.ShardingCommon.Core.Strategy.Route.Value
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/16 11:15:11
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public class RangeRouteValue : IRouteValue
    {
        private readonly string _columnName;
    
        private readonly string _tableName;
    
        private readonly Range<IComparable> _valueRange;

        public RangeRouteValue(string columnName, string tableName, Range<IComparable> valueRange)
        {
            _columnName = columnName;
            _tableName = tableName;
            _valueRange = valueRange;
        }

        public string GetColumnName()
        {
            return _columnName;
        }

        public string GetTableName()
        {
            return _tableName;
        }


        public Range<IComparable> GetValueRange()
        {
            return _valueRange;
        }
    }
}