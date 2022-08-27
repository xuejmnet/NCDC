using System;
using ShardingConnector.Executor;

namespace ShardingConnector.Merge.Reader.Transparent
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/16 17:13:38
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public sealed class TransparentMergedDataReader : IStreamDataReader
    {
        private readonly IStreamDataReader _streamDataReader;

        public TransparentMergedDataReader(IStreamDataReader streamDataReader)
        {
            this._streamDataReader = streamDataReader;
        }


        public bool Read()
        {
            return _streamDataReader.Read();
        }

        public int ColumnCount => _streamDataReader.ColumnCount;

        public string GetColumnName(int columnIndex)
        {
            return _streamDataReader.GetColumnName(columnIndex);
        }

        public string GetColumnLabel(int columnIndex)
        {
            return _streamDataReader.GetColumnLabel(columnIndex);
        }

        public object this[int columnIndex] => GetValue(columnIndex);

        public object this[string name] => GetValue(GetOrdinal(name));

        public string GetName(int columnIndex)
        {
            return _streamDataReader.GetName(columnIndex);
        }

        public string GetDataTypeName(int columnIndex)
        {
            return _streamDataReader.GetDataTypeName(columnIndex);
        }

        public Type GetFieldType(int columnIndex)
        {
            return _streamDataReader.GetFieldType(columnIndex);
        }

        public object GetValue(int columnIndex)
        {
            return _streamDataReader.GetValue(columnIndex);
        }

        public int GetValues(object[] values)
        {
            return _streamDataReader.GetValues(values);
        }

        public int GetOrdinal(string name)
        {
            return _streamDataReader.GetOrdinal(name);
        }

        public bool GetBoolean(int columnIndex)
        {
            return _streamDataReader.GetBoolean(columnIndex);
        }

        public byte GetByte(int columnIndex)
        {
            return _streamDataReader.GetByte(columnIndex);
        }


        public long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
        {
            return _streamDataReader.GetBytes(ordinal, dataOffset, buffer, bufferOffset, length);
        }

        public char GetChar(int columnIndex)
        {
            return _streamDataReader.GetChar(columnIndex);
        }

        public long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
        {
            return _streamDataReader.GetChars(ordinal, dataOffset, buffer, bufferOffset, length);
        }

        public Guid GetGuid(int columnIndex)
        {
            return _streamDataReader.GetGuid(columnIndex);
        }

        public short GetInt16(int columnIndex)
        {
            return _streamDataReader.GetInt16(columnIndex);
        }

        public int GetInt32(int columnIndex)
        {
            return _streamDataReader.GetInt32(columnIndex);
        }

        public long GetInt64(int columnIndex)
        {
            return _streamDataReader.GetInt64(columnIndex);
        }

        public float GetFloat(int columnIndex)
        {
            return _streamDataReader.GetFloat(columnIndex);
        }

        public double GetDouble(int columnIndex)
        {
            return _streamDataReader.GetDouble(columnIndex);
        }

        public string GetString(int columnIndex)
        {
            return _streamDataReader.GetString(columnIndex);
        }

        public decimal GetDecimal(int columnIndex)
        {
            return _streamDataReader.GetDecimal(columnIndex);
        }

        public DateTime GetDateTime(int columnIndex)
        {
            return _streamDataReader.GetDateTime(columnIndex);
        }

        public bool IsDBNull(int columnIndex)
        {
            return _streamDataReader.IsDBNull(columnIndex);
        }

        public bool NextResult()
        {
            return _streamDataReader.NextResult();
        }

        public void Dispose()
        {
            _streamDataReader?.Dispose();
        }
    }
}