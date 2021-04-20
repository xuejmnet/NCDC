using System;
using System.Data.Common;
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

        public MemoryQueryDataReader(DbDataReader dataReader)
        {
            _dataReader = dataReader;
        }
        public bool MoveNext()
        {
            throw new NotImplementedException();
        }

        public object GetValue(int columnIndex)
        {
            throw new NotImplementedException();
        }

        public T GetValue<T>(int columnIndex)
        {
            throw new NotImplementedException();
        }

        public int ColumnCount { get; }
        public string GetColumnName(int columnIndex)
        {
            throw new NotImplementedException();
        }

        public string GetColumnLabel(int columnIndex)
        {
            throw new NotImplementedException();
        }

        public bool IsDBNull(int columnIndex)
        {
            throw new NotImplementedException();
        }
    }
}