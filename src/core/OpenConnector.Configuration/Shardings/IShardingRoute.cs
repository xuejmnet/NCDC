namespace OpenConnector.Shardings;

public interface IShardingRoute
{
    /// <summary>
    /// 分片路由
    /// </summary>
    /// <param name="sql">数据库值</param>
    /// <returns></returns>
    ICollection<string> DoSharding(string sql);
}