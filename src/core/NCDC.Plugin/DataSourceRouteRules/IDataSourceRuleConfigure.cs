using NCDC.Plugin.TableRouteRules;

namespace NCDC.Plugin.DataSourceRouteRules;

public interface IDataSourceRuleConfigure
{
    /// <summary>
    /// 配置对象的分库信息
    /// </summary>
    /// <param name="builder"></param>
    void Configure(DataSourceRuleConfigureBuilder builder);
}