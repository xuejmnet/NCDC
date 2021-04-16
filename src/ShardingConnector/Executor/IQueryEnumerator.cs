namespace ShardingConnector.Executor
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
    public interface IQueryEnumerator
    {
        bool MoveNext();
        object GetValue(int columnIndex);
        T GetValue<T>(int columnIndex);
        int ColumnCount { get; }
        string GetColumnName(int columnIndex);
        string GetColumnLabel(int columnIndex);
        bool IsDBNull(int columnIndex);
    }
}