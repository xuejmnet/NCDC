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
    public sealed class TransparentMergedDataReader : IMergedDataReader
    {
        private readonly IQueryDataReader _queryDataReader;

        public TransparentMergedDataReader(IQueryDataReader queryDataReader)
        {
            this._queryDataReader = queryDataReader;
        }

        public bool Read()
        {
            return _queryDataReader.Read();
        }

        public object GetValue(int columnIndex)
        {
            return _queryDataReader.GetValue(columnIndex);
        }

        public T GetValue<T>(int columnIndex)
        {
            return _queryDataReader.GetValue<T>(columnIndex);
        }

        public object GetValue(string columnName)
        {
            return _queryDataReader.GetValue(columnName);
        }

        public T GetValue<T>(string columnName)
        {
            return _queryDataReader.GetValue<T>(columnName);
        }

        public long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
        {
            return _queryDataReader.GetBytes(ordinal, dataOffset, buffer, bufferOffset, length);
        }

        public long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
        {
            return _queryDataReader.GetChars(ordinal, dataOffset, buffer, bufferOffset, length);
        }

        public bool IsDBNull(int columnIndex)
        {
            return _queryDataReader.IsDBNull(columnIndex);
        }

        public bool NextResult()
        {
            return _queryDataReader.NextResult();
        }
    }
}