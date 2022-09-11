namespace OpenConnector.Metadatas;

public interface ITableMetadata
{
    /// <summary>
    /// 获取所有的列名称
    /// </summary>
    /// <returns></returns>
    IDictionary<string/*列名称*/, IColumnMetadata> GetColumns();
    /// <summary>
    /// 获取所有的索引
    /// </summary>
    /// <returns></returns>
    IDictionary<string/*列名称*/, IIndexMetadata> GetIndexes();
    /// <summary>
    /// 获取列信息
    /// </summary>
    /// <param name="columnIndex"></param>
    /// <returns></returns>
    IColumnMetadata? GetColumnMetaData(int columnIndex);
    /// <summary>
    /// 获取列信息
    /// </summary>
    /// <param name="columnName"></param>
    /// <returns>null表示找不到</returns>
    IColumnMetadata? GetColumnMetaData(string columnName);
    /// <summary>
    /// 返回列名所在索引位置
    /// </summary>
    /// <param name="columnName"></param>
    /// <returns>-1表示不存在</returns>
    int FindColumnIndex(string columnName);

  
    /// <summary>
    /// 当前索引是否是key
    /// </summary>
    /// <param name="columnIndex"></param>
    /// <returns></returns>
    bool IsPrimaryKey(int columnIndex);
    /// <summary>
    /// 当前索引是否是key
    /// </summary>
    /// <param name="columnName"></param>
    /// <returns></returns>
    bool IsPrimaryKey(string columnName);
}
