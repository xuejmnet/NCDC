//using System.Threading;
//using System.Threading.Tasks;

//namespace ShardingConnector.Merge.Reader
//{
//    /*
//    * @Author: xjm
//    * @Description:
//    * @Date: 2021/04/16 00:00:00
//    * @Ver: 1.0
//    * @Email: 326308290@qq.com
//    */
//    /// <summary>
//    /// 
//    /// </summary>
//    public interface IMergedDataReader
//    {
//        bool Read();
//        object GetValue(int columnIndex);
//        T GetValue<T>(int columnIndex);
//        object GetValue(string columnName);
//        T GetValue<T>(string columnName);


//        long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length);


//        long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length);

//        bool IsDBNull(int columnIndex);
//        bool NextResult();
//    }

//}