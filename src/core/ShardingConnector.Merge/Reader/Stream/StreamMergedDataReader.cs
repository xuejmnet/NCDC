using System;
using ShardingConnector.Exceptions;
using ShardingConnector.Executor;

namespace ShardingConnector.Merge.Reader.Stream
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/16 16:42:45
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public abstract class StreamMergedDataReader : IStreamDataReader
    {
        private IStreamDataReader _current;

        public abstract bool Read();
        public int ColumnCount => CurrentQueryDataReader.ColumnCount;
        public string GetColumnName(int columnIndex)
        {
            return CurrentQueryDataReader.GetColumnName(columnIndex);
        }

        public string GetColumnLabel(int columnIndex)
        {
            return CurrentQueryDataReader.GetColumnLabel(columnIndex);
        }

        public object this[int columnIndex] => GetValue(columnIndex);

        public object this[string name] => GetValue(GetOrdinal(name));

        public string GetName(int columnIndex)
        {
            return CurrentQueryDataReader.GetName(columnIndex);
        }

        public string GetDataTypeName(int columnIndex)
        {
            return CurrentQueryDataReader.GetDataTypeName(columnIndex);
        }

        public Type GetFieldType(int columnIndex)
        {
            return CurrentQueryDataReader.GetFieldType(columnIndex);
        }

        public virtual object GetValue(int columnIndex)
        {
            return CurrentQueryDataReader.GetValue(columnIndex);
        }

        public int GetValues(object[] values)
        {
            return CurrentQueryDataReader.GetValues(values);
        }

        public int GetOrdinal(string name)
        {
            return CurrentQueryDataReader.GetOrdinal(name);
        }

        public bool GetBoolean(int columnIndex)
        {
            return CurrentQueryDataReader.GetBoolean(columnIndex);
        }

        public byte GetByte(int columnIndex)
        {
            return CurrentQueryDataReader.GetByte(columnIndex);
        }


        public long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
        {
            return CurrentQueryDataReader.GetBytes(ordinal, dataOffset, buffer, bufferOffset, length);
        }

        public char GetChar(int columnIndex)
        {
            return CurrentQueryDataReader.GetChar(columnIndex);
        }

        public long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
        {
            return CurrentQueryDataReader.GetChars(ordinal, dataOffset, buffer, bufferOffset, length);
        }

        public Guid GetGuid(int columnIndex)
        {
            return CurrentQueryDataReader.GetGuid(columnIndex);
        }

        public short GetInt16(int columnIndex)
        {
            return CurrentQueryDataReader.GetInt16(columnIndex);
        }

        public int GetInt32(int columnIndex)
        {
            return CurrentQueryDataReader.GetInt32(columnIndex);
        }

        public long GetInt64(int columnIndex)
        {
            return CurrentQueryDataReader.GetInt64(columnIndex);
        }

        public float GetFloat(int columnIndex)
        {
            return CurrentQueryDataReader.GetFloat(columnIndex);
        }

        public double GetDouble(int columnIndex)
        {
            return CurrentQueryDataReader.GetDouble(columnIndex);
        }

        public string GetString(int columnIndex)
        {
            return CurrentQueryDataReader.GetString(columnIndex);
        }

        public decimal GetDecimal(int columnIndex)
        {
            return CurrentQueryDataReader.GetDecimal(columnIndex);
        }

        public DateTime GetDateTime(int columnIndex)
        {
            return CurrentQueryDataReader.GetDateTime(columnIndex);
        }

        public virtual bool IsDBNull(int columnIndex)
        {
            return CurrentQueryDataReader.IsDBNull(columnIndex);
        }

        public bool NextResult()
        {
            return CurrentQueryDataReader.NextResult();
        }

        protected IStreamDataReader GetCurrentStreamDataReader()
        {
            if (null == _current)
            {
                throw new ShardingException("Current Stream DataReader is null, DataReader perhaps end of read.");
            }

            return _current;
        }

        public void SetCurrentStreamDataReader(IStreamDataReader streamDataReader)
        {
            _current = streamDataReader;
        }

        protected IStreamDataReader CurrentQueryDataReader => GetCurrentStreamDataReader();

    }
}