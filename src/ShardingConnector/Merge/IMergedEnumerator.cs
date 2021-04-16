namespace ShardingConnector.Merge
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/16 00:00:00
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    /// <summary>
    /// 
    /// </summary>
    public interface IMergedEnumerator
    {
        bool MoveNext();
        object GetValue(int columnIndex);
        T GetValue<T>(int columnIndex);

        // object GetValue(int columnIndex, long dataOffset, char[] buffer, int bufferOffset, int length);
        // T GetValue<T>(int columnIndex, long dataOffset, char[] buffer, int bufferOffset, int length);

        bool IsDBNull(int columnIndex);
    }

}