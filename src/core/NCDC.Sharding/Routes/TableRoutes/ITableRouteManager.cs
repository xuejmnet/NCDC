// using NCDC.Sharding.Routes.Abstractions;
//
// namespace NCDC.Sharding.Routes.TableRoutes;
//
// public interface ITableRouteManager
// {
//     
//     /// <summary>
//     /// 实体对象是否存在分表路由
//     /// </summary>
//     /// <param name="tableName"></param>
//     /// <returns></returns>
//     bool HasRoute(string tableName);
//     /// <summary>
//     /// 获取实体对象的分表路由,如果没有将抛出异常
//     /// </summary>
//     /// <param name="tableName"></param>
//     /// <returns></returns>
//     /// <exception cref="ShardingInvalidOperationException">如果没有找到对应的路由</exception>
//     ITableRoute GetRoute(string tableName);
//     /// <summary>
//     /// 获取所有的分表路由
//     /// </summary>
//     /// <returns></returns>
//     List<ITableRoute> GetRoutes();
//     /// <summary>
//     /// 添加分表路由
//     /// </summary>
//     /// <param name="route"></param>
//     /// <returns></returns>
//     /// <exception cref="ShardingInvalidOperationException">如果当前路由的对象不是分表对象将抛出异常</exception>
//     bool AddRoute(ITableRoute route);
//
//     /// <summary>
//     /// 直接路由采用默认数据源
//     /// </summary>
//     /// <param name="tableName"></param>
//     /// <param name="sqlParserResult"></param>
//     /// <returns></returns>
//     List<TableRouteUnit> RouteTo(string tableName,SqlParserResult sqlParserResult);
// }