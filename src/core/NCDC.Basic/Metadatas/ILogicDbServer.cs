using NCDC.Basic.Connection.User;

namespace NCDC.Basic.Metadatas;

/// <summary>
/// 逻辑数据库服务器
/// </summary>
public interface ILogicDbServer
{
    // /// <summary>
    // /// 逻辑数据库
    // /// </summary>
    // IReadOnlyDictionary<string/*逻辑数据库名称*/,ILogicDatabase> LogicDatabases { get; }

    /// <summary>
    /// 根据逻辑数据库名称获取逻辑数据库
    /// </summary>
    /// <param name="logicDatabaseName"></param>
    /// <returns></returns>
    ILogicDatabase? GetLogicDatabase(string logicDatabaseName);

    /// <summary>
    /// 获取所有的逻辑数据库
    /// </summary>
    /// <returns></returns>
    IEnumerable<string> GetAllLogicDatabaseNames();

    /// <summary>
    /// 是否包含对应的逻辑数据库名称
    /// </summary>
    /// <param name="logicDatabaseName"></param>
    /// <returns></returns>
    bool ContainsLogicDatabase(string logicDatabaseName);

    /// <summary>
    /// 创建逻辑数据库
    /// </summary>
    /// <param name="logicDatabase"></param>
    /// <returns></returns>
    bool CreateLogicDatabase(ILogicDatabase logicDatabase);

    /// <summary>
    /// 删除逻辑数据库
    /// </summary>
    /// <param name="logicDatabaseName"></param>
    /// <returns></returns>
    bool DropLogicDatabase(string logicDatabaseName);


    bool DatabaseExists(string? database);

    ILogicDatabase? GetDatabase(string? database);

    OpenConnectorUser? GetUser(string username);
}