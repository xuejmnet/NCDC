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
    public sealed class SingleLocalDataMergedDataReader:IMergedDataReader
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

        public object GetValue(int columnIndex)
        {
            return _currentValue;
        }

        public T GetValue<T>(int columnIndex)
        {
            return (T) _currentValue;
        }

        public object GetValue(string columnName)
        {
            return _currentValue;
        }

        public T GetValue<T>(string columnName)
        {
            return (T)_currentValue;
        }

        public long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
        {
            return (long)_currentValue;
        }

        public long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
        {
            return (long)_currentValue;
        }

        public bool IsDBNull(int columnIndex)
        {
            return null == _currentValue;
        }

        public bool NextResult()
        {
            throw new NotImplementedException();
        }
    }
}
