using System;
using System.Collections.Generic;
using System.Linq;

namespace ShardingConnector.ShardingCommon.Core.Strategy.Route.Value
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/16 11:19:20
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public class ListRouteValue<T>:IRouteValue where T:IComparable
    {
        private readonly string _columnName;
    
        private readonly string _tableName;
    
        private readonly ICollection<T> _values;

        public ListRouteValue(string columnName, string tableName, ICollection<T> values)
        {
            _columnName = columnName;
            _tableName = tableName;
            _values = values;
        }

        public string GetColumnName()
        {
            return _columnName;
        }

        public string GetTableName()
        {
            return _tableName;
        }

        public ICollection<T> GetValues()
        {
            return _values;
        }

        public override string ToString()
        {
            return $"{_tableName}.{_columnName} {(1 == _values.Count ? _values.First().ToString() : GetInToString())}";
        }

        public string GetInToString()
        {
            return $"in ( {string.Join(",", _values)} )";
        }
    }
}