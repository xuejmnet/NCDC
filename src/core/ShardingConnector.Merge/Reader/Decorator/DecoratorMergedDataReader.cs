using System.Threading;
using System.Threading.Tasks;

namespace ShardingConnector.Merge.Reader.Decorator
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
    public abstract class DecoratorMergedDataReader:IMergedDataReader
    {
        public  IMergedDataReader MergedDataReader { get; }

        protected DecoratorMergedDataReader(IMergedDataReader mergedDataReader)
        {
            MergedDataReader = mergedDataReader;
        }

        public virtual bool Read()
        {
            return MergedDataReader.Read();
        }

        public object GetValue(int columnIndex)
        {
            return MergedDataReader.GetValue(columnIndex);
        }

        public T GetValue<T>(int columnIndex)
        {
            return MergedDataReader.GetValue<T>(columnIndex);
        }

        public object GetValue(string columnName)
        {
            return MergedDataReader.GetValue(columnName);
        }

        public T GetValue<T>(string columnName)
        {
            return MergedDataReader.GetValue<T>(columnName);
        }

        public long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
        {
            return MergedDataReader.GetBytes(ordinal, dataOffset, buffer, bufferOffset, length);
        }

        public long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
        {
            return MergedDataReader.GetChars(ordinal, dataOffset, buffer, bufferOffset, length);
        }

        public bool IsDBNull(int columnIndex)
        {
            return MergedDataReader.IsDBNull(columnIndex);
        }

        public bool NextResult()
        {
            return MergedDataReader.NextResult();
        }
    }
}