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
    public class MemoryQueryDataReader:IQueryDataReader
    {
        private readonly DbDataReader _dataReader;
        private readonly List<DbColumn> _columnSchema;

        public MemoryQueryDataReader(DbDataReader dataReader)
        {
            _columnSchema = dataReader.GetColumnSchema().ToList();
            _dataReader = dataReader;
        }
        public bool Read()
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

        public object GetValue(string columnName)
        {
            return _dataReader[columnName];
        }

        public T GetValue<T>(string columnName)
        {
            return (T)_dataReader[columnName];
        }

        public int ColumnCount => _dataReader.FieldCount;

        /// <summary>
        /// select name as label from table
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <returns>name</returns>

        public string GetColumnName(int columnIndex)
        {
            return _columnSchema[columnIndex].BaseColumnName;
        }

        /// <summary>
        /// select name as label from table
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <returns>label</returns>
        public string GetColumnLabel(int columnIndex)
        {
            return _columnSchema[columnIndex].ColumnName;
        }

        public bool IsDBNull(int columnIndex)
        {
            return _dataReader.IsDBNull(columnIndex);
        }

        public long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
        {
            return _dataReader.GetBytes(ordinal, dataOffset, buffer, bufferOffset, length);
        }

        public long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
        {
            return _dataReader.GetChars(ordinal, dataOffset, buffer, bufferOffset, length);
        }

        public bool NextResult()
        {
            return _dataReader.NextResult();
        }
    }
}