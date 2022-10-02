namespace NCDC.Plugin.DataSourceRouteRules;

public class DataSourceRuleConfigureBuilder
{
    private readonly TableConfiguration _tableConfiguration;

    private DataSourceRuleConfigureBuilder(TableConfiguration tableConfiguration)
    {
        _tableConfiguration = tableConfiguration;
    }
    /// <summary>
    /// 设置分表字段
    /// </summary>
    /// <param name="columnName"></param>
    /// <returns></returns>
    public DataSourceRuleConfigureBuilder ShardingColumn(string columnName)
    {
        _tableConfiguration.SetShardingTableColumn(columnName);
        return this;
    }
    public DataSourceRuleConfigureBuilder AddShardingExtraColumn(string columnName)
    {
        _tableConfiguration.AddExtraSharingTableColumn(columnName);
        return this;
    }
    /// <summary>
    /// 创建分表元数据建造者
    /// </summary>
    /// <param name="tableConfiguration"></param>
    /// <returns></returns>
    public static DataSourceRuleConfigureBuilder CreateDataSourceRuleConfigureBuilder(TableConfiguration tableConfiguration)
    {
        return new DataSourceRuleConfigureBuilder(tableConfiguration);
    }
}