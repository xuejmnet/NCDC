using NCDC.Plugin.DataSourceRouteRules;
using NCDC.Plugin.TableRouteRules;

namespace NCDC.ProxyServer.Options;

public interface IRouteInitConfigOption
{ 
   /// <summary>
   /// 添加分库路由规则
   /// </summary>
   /// <param name="logicTable"></param>
   /// <param name="routeRuleTypeClass"></param>
    void AddDataSourceRouteRule(string logicTable,string routeRuleTypeClass);

    /// <summary>
    /// 是否有分库路由规则
    /// </summary>
    /// <param name="logicTable"></param>
    /// <returns></returns>
    bool HasDataSourceRouteRule(string logicTable);

    /// <summary>
    /// 获取分库路由规则类型
    /// </summary>
    /// <param name="logicTable"></param>
    /// <returns></returns>
    string GetDataSourceRouteRule(string logicTable);

    /// <summary>
    /// 添加分表路由规则
    /// </summary>
    /// <param name="logicTable">逻辑表名</param>
    /// <param name="routeRuleTypeClass">路由规则</param>
    void AddTableRouteRule(string logicTable,string routeRuleTypeClass);

    /// <summary>
    /// 是否有分表表路由规则
    /// </summary>
    /// <param name="logicTable"></param>
    /// <returns></returns>
    bool HasTableRouteRule(string logicTable);

    /// <summary>
    /// 获取分表路由规则类型
    /// </summary>
    /// <param name="logicTable"></param>
    /// <returns></returns>
    string GetTableRouteRule(string logicTable);
    
    /// <summary>
    /// 获取所有的分库路由规则类型
    /// </summary>
    /// <returns></returns>
    IReadOnlyDictionary<string/*logic table name*/,string> GetDataSourceRouteRules();

    /// <summary>
    /// 获取所有的分表路由规则类型
    /// </summary>
    /// <returns></returns>
    IReadOnlyDictionary<string/*logic table name*/,string> GetTableRouteRules();

}