using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using ShardingConnector.Executor;

namespace ShardingConnector.ShardingExecute.Execute.DataReader
{
/*
* @Author: xjm
* @Description:
* @Date: Saturday, 17 April 2021 17:48:21
* @Email: 326308290@qq.com
*/
    public class MemoryQueryDataReader:IQueryEnumerator
    {
        private readonly DbDataReader _dataReader;
        private readonly List<DbColumn> _columnSchema;

        public MemoryQueryDataReader(DbDataReader dataReader)
        {
            _columnSchema = dataReader.GetColumnSchema().ToList();
            _dataReader = dataReader;
        }
        public bool MoveNext()
        {
            return _dataReader.Read();
        }

        public object GetValue(int columnIndex)
        {
           return _dataReader[columnIndex];
        }

        public T GetValue<T>(int columnIndex)
        {
            return (T)_dataReader[columnIndex];
        }

        public int ColumnCount => _dataReader.FieldCount;
        public string GetColumnName(int columnIndex)
        {
            return _columnSchema[columnIndex].BaseColumnName;
        }

        public string GetColumnLabel(int columnIndex)
        {
            return _columnSchema[columnIndex].ColumnName;
        }

        public bool IsDBNull(int columnIndex)
        {
            return _dataReader.IsDBNull(columnIndex);
        }
    }
}