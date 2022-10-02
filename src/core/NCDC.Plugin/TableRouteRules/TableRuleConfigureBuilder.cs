namespace NCDC.Plugin.TableRouteRules;

public class TableRuleConfigureBuilder
{
    private readonly TableConfiguration _tableConfiguration;

    private TableRuleConfigureBuilder(TableConfiguration tableConfiguration)
    {
        _tableConfiguration = tableConfiguration;
    }
    /// <summary>
    /// 设置分表字段
    /// </summary>
    /// <param name="columnName"></param>
    /// <returns></returns>
    public TableRuleConfigureBuilder ShardingColumn(string columnName)
    {
        _tableConfiguration.SetShardingTableColumn(columnName);
        return this;
    }
    public TableRuleConfigureBuilder AddShardingExtraColumn(string columnName)
    {
        _tableConfiguration.AddExtraSharingTableColumn(columnName);
        return this;
    }

    public static TableRuleConfigureBuilder CreateTableRuleConfigureBuilder(TableConfiguration tableConfiguration)
    {
        return new TableRuleConfigureBuilder(tableConfiguration);
    }
}