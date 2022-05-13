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
    public abstract class StreamMergedDataReader : IMergedDataReader
    {
        private IQueryDataReader _current;

        public abstract bool Read();

        public object GetValue(int columnIndex)
        {
            return CurrentQueryDataReader.GetValue(columnIndex);
        }

        public object GetValue(string columnName)
        {
            return CurrentQueryDataReader.GetValue(columnName);
        }

        public T GetValue<T>(string columnName)
        {
            return CurrentQueryDataReader.GetValue<T>(columnName);
        }

        public long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
        {
            return CurrentQueryDataReader.GetBytes(ordinal, dataOffset, buffer, bufferOffset, length);
        }

        public long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
        {
            return CurrentQueryDataReader.GetChars(ordinal, dataOffset, buffer, bufferOffset, length);
        }

        public T GetValue<T>(int columnIndex)
        {
            return CurrentQueryDataReader.GetValue<T>(columnIndex);
        }

        public bool IsDBNull(int columnIndex)
        {
            return CurrentQueryDataReader.IsDBNull(columnIndex);
        }

        public bool NextResult()
        {
            return CurrentQueryDataReader.NextResult();
        }

        protected IQueryDataReader GetCurrentQueryDataReader()
        {
            if (null == _current)
            {
                throw new ShardingException("Current DataReader is null, DataReader perhaps end of read.");
            }

            return _current;
        }

        public void SetCurrentQueryEnumerator(IQueryDataReader queryDataReader)
        {
            _current = queryDataReader;
        }

        protected IQueryDataReader CurrentQueryDataReader => GetCurrentQueryDataReader();

    }
}