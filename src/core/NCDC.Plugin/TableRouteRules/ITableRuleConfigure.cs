namespace NCDC.Plugin.TableRouteRules;

public interface ITableRuleConfigure
{
    /// <summary>
    /// 配置对象的分表信息
    /// </summary>
    /// <param name="builder"></param>
    void Configure(TableRuleConfigureBuilder builder);
}