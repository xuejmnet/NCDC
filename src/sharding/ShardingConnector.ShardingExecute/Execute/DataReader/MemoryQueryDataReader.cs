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
    public class MemoryQueryDataReader:IStreamDataReader
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

        public Type GetFieldType(int columnIndex)
        {
            return _dataReader.GetFieldType(columnIndex);
        }

        public object GetValue(int columnIndex)
        {
           return _dataReader[columnIndex];
        }

        public int GetValues(object[] values)
        {
            return _dataReader.GetValues(values);
        }

        public int GetOrdinal(string name)
        {
            return _dataReader.GetOrdinal(name);
        }

        public bool GetBoolean(int columnIndex)
        {
            return _dataReader.GetBoolean(columnIndex);

        }

        public byte GetByte(int columnIndex)
        {
            return _dataReader.GetByte(columnIndex);
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

        public object this[int columnIndex] => GetValue(columnIndex);

        public object this[string name] => GetValue(GetOrdinal(name));

        public string GetName(int columnIndex)
        {
            return _dataReader.GetName(columnIndex);
        }

        public string GetDataTypeName(int columnIndex)
        {
            return _dataReader.GetDataTypeName(columnIndex);
        }

        public DateTime GetDateTime(int columnIndex)
        {
            return _dataReader.GetDateTime(columnIndex);
        }

        public bool IsDBNull(int columnIndex)
        {
            return _dataReader.IsDBNull(columnIndex);
        }

        public long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
        {
            return _dataReader.GetBytes(ordinal, dataOffset, buffer, bufferOffset, length);
        }

        public char GetChar(int columnIndex)
        {
            return _dataReader.GetChar(columnIndex);
        }

        public long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
        {
            return _dataReader.GetChars(ordinal, dataOffset, buffer, bufferOffset, length);
        }

        public Guid GetGuid(int columnIndex)
        {
            return _dataReader.GetGuid(columnIndex);
        }

        public short GetInt16(int columnIndex)
        {
            return _dataReader.GetInt16(columnIndex);
        }

        public int GetInt32(int columnIndex)
        {
            return _dataReader.GetInt32(columnIndex);
        }

        public long GetInt64(int columnIndex)
        {
            return _dataReader.GetInt64(columnIndex);
        }

        public float GetFloat(int columnIndex)
        {
            return _dataReader.GetFloat(columnIndex);
        }

        public double GetDouble(int columnIndex)
        {
            return _dataReader.GetDouble(columnIndex);
        }

        public string GetString(int columnIndex)
        {
            return _dataReader.GetString(columnIndex);
        }

        public decimal GetDecimal(int columnIndex)
        {
            return _dataReader.GetDecimal(columnIndex);
        }

        public bool NextResult()
        {
            return _dataReader.NextResult();
        }
    }
}