using NCDC.StreamDataReaders;

namespace NCDC.ShardingMerge.DataReaders.Decorator
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/16 00:00:00
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    /// <summary>
    /// 笛卡尔积的合并
    /// </summary>
    public abstract class DecoratorStreamDataReader:IStreamDataReader
    {
        public IStreamDataReader StreamDataReader { get; }

        protected DecoratorStreamDataReader(IStreamDataReader streamDataReader)
        {
            StreamDataReader = streamDataReader;
        }


        public virtual bool Read()
        {
            return StreamDataReader.Read();
        }

        public int ColumnCount => StreamDataReader.ColumnCount;
        public string GetColumnName(int columnIndex)
        {
            return StreamDataReader.GetColumnName(columnIndex);
        }

        public string GetColumnLabel(int columnIndex)
        {
            return StreamDataReader.GetColumnLabel(columnIndex);
        }

        public object this[int columnIndex] => StreamDataReader[columnIndex];

        public object this[string name] => StreamDataReader[name];

        public string GetName(int columnIndex)
        {
            return StreamDataReader.GetName(columnIndex);
        }

        public string GetDataTypeName(int columnIndex)
        {
            return StreamDataReader.GetDataTypeName(columnIndex);
        }

        public Type GetFieldType(int columnIndex)
        {
            return StreamDataReader.GetFieldType(columnIndex);
        }

        public object GetValue(int columnIndex)
        {
            return StreamDataReader.GetValue(columnIndex);
        }

        public int GetValues(object[] values)
        {
            return StreamDataReader.GetValues(values);
        }

        public int GetOrdinal(string name)
        {
            return StreamDataReader.GetOrdinal(name);
        }

        public bool GetBoolean(int columnIndex)
        {
            return StreamDataReader.GetBoolean(columnIndex);
        }

        public byte GetByte(int columnIndex)
        {
            return StreamDataReader.GetByte(columnIndex);
        }

        public long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
        {
            return StreamDataReader.GetBytes(ordinal, dataOffset, buffer, bufferOffset, length);
        }

        public char GetChar(int columnIndex)
        {
            return StreamDataReader.GetChar(columnIndex);
        }

        public long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
        {
            return StreamDataReader.GetChars(ordinal, dataOffset, buffer, bufferOffset, length);
        }

        public Guid GetGuid(int columnIndex)
        {
            return StreamDataReader.GetGuid(columnIndex);
        }

        public short GetInt16(int columnIndex)
        {
            return StreamDataReader.GetInt16(columnIndex);
        }

        public int GetInt32(int columnIndex)
        {
            return StreamDataReader.GetInt32(columnIndex);
        }

        public long GetInt64(int columnIndex)
        {
            return StreamDataReader.GetInt64(columnIndex);
        }

        public float GetFloat(int columnIndex)
        {
            return StreamDataReader.GetFloat(columnIndex);
        }

        public double GetDouble(int columnIndex)
        {
            return StreamDataReader.GetDouble(columnIndex);
        }

        public string GetString(int columnIndex)
        {
            return StreamDataReader.GetString(columnIndex);
        }

        public decimal GetDecimal(int columnIndex)
        {
            return StreamDataReader.GetDecimal(columnIndex);
        }

        public DateTime GetDateTime(int columnIndex)
        {
            return StreamDataReader.GetDateTime(columnIndex);
        }

        public bool IsDBNull(int columnIndex)
        {
            return StreamDataReader.IsDBNull(columnIndex);
        }

        public bool NextResult()
        {
            return StreamDataReader.NextResult();
        }

        public void Dispose()
        {
            StreamDataReader?.Dispose();
        }
    }
}