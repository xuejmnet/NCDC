using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.Merge.Reader;

namespace ShardingConnector.ShardingMerge.DAL.Common
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/5/5 10:39:24
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class SingleLocalDataMergedDataReader:IStreamDataReader
    {
        private readonly IEnumerator<object> _values;

        private object _currentValue;

        public SingleLocalDataMergedDataReader(ICollection<object> values)
        {
            this._values = values.GetEnumerator();
        }



        public bool Read()
        {
            if (_values.MoveNext())
            {
                _currentValue = _values.Current;
                return true;
            }
            return false;
        }

        public int ColumnCount => 1;
        public string GetColumnName(int columnIndex)
        {
            throw new NotImplementedException();
        }

        public string GetColumnLabel(int columnIndex)
        {
            throw new NotImplementedException();
        }

        public object this[int columnIndex] => GetValue(columnIndex);

        public object this[string name] => GetValue(GetOrdinal(name));

        public string GetName(int columnIndex)
        {
            throw new NotImplementedException();
        }

        public string GetDataTypeName(int columnIndex)
        {
            throw new NotImplementedException();
        }

        public Type GetFieldType(int columnIndex)
        {
            throw new NotImplementedException();
        }

        public object GetValue(int columnIndex)
        {
            return _currentValue;
        }

        public int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public int GetOrdinal(string name)
        {
            return 0;
        }

        public bool GetBoolean(int columnIndex)
        {
            return (bool)_currentValue;
        }

        public byte GetByte(int columnIndex)
        {
            return (byte)_currentValue;
        }
        

        public long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
        {
            return (long)_currentValue;
        }

        public char GetChar(int columnIndex)
        {
            return (char)_currentValue;
        }

        public long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
        {
            return (long)_currentValue;
        }

        public Guid GetGuid(int columnIndex)
        {
            return (Guid)_currentValue;
        }

        public short GetInt16(int columnIndex)
        {
            return (short)_currentValue;
        }

        public int GetInt32(int columnIndex)
        {
            return (int)_currentValue;
        }

        public long GetInt64(int columnIndex)
        {
            return (long)_currentValue;
        }

        public float GetFloat(int columnIndex)
        {
            return (float)_currentValue;
        }

        public double GetDouble(int columnIndex)
        {
            return (double)_currentValue;
        }

        public string GetString(int columnIndex)
        {
            return (string)_currentValue;
        }

        public decimal GetDecimal(int columnIndex)
        {
            return (decimal)_currentValue;
        }

        public DateTime GetDateTime(int columnIndex)
        {
            return (DateTime)_currentValue;
        }

        public bool IsDBNull(int columnIndex)
        {
            return null == _currentValue;
        }

        public bool NextResult()
        {
            return false;
        }
    }
}
