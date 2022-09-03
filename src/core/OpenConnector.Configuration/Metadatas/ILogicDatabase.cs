namespace OpenConnector.Configuration.Metadatas;

/// <summary>
/// 逻辑数据库
/// </summary>
public interface ILogicDatabase
{
    /// <summary>
    /// 逻辑数据库名称
    /// </summary>
    string Name { get; }
}