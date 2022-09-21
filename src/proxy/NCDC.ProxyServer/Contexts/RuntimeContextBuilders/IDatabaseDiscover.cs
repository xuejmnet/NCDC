namespace NCDC.ProxyServer.Contexts.RuntimeContextBuilders;

public interface IDatabaseDiscover
{
   /// <summary>
   /// 获取数据库名称
   /// </summary>
   /// <returns>返回数据库名称</returns>
    string GetDatabaseName();

   /// <summary>
   /// 路由的标识
   /// </summary>
   /// <returns></returns>
   IReadOnlyList<string> RouteIdentities();

   /// <summary>
   /// 数据源
   /// </summary>
   /// <returns></returns>
   IReadOnlyList<IDataSourceDiscover> GetDataSources();

}