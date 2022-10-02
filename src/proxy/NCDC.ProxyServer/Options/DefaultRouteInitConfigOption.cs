using System.Collections.Immutable;
using NCDC.Exceptions;

namespace NCDC.ProxyServer.Options;

public class DefaultRouteInitConfigOption:IRouteInitConfigOption
{
    private readonly IDictionary<string, string> _dataSourceRouteRules = new Dictionary<string, string>();
    private readonly IDictionary<string, string> _tableRouteRules = new Dictionary<string, string>();
    public void AddDataSourceRouteRule(string logicTable,string routeRuleTypeClass)
    {
        if (_dataSourceRouteRules.TryGetValue(logicTable, out var ruleTypeClass))
        {
            throw new ShardingInvalidOperationException(
                $"table:{logicTable},already use data source route rule:[{ruleTypeClass}]");
        }

        _dataSourceRouteRules.TryAdd(logicTable, routeRuleTypeClass);
    }

    public bool HasDataSourceRouteRule(string logicTable)
    {
        return _dataSourceRouteRules.ContainsKey(logicTable);
    }

    public string GetDataSourceRouteRule(string logicTable)
    {
       
        if (!_dataSourceRouteRules.TryGetValue(logicTable, out var dataSourceRouteRule))
        {
            throw new ShardingInvalidOperationException(
                $"table:{logicTable},data source route rule not found");
        }

        return dataSourceRouteRule;
    }

    public void AddTableRouteRule(string logicTable,string routeRuleTypeClass)
    {
      
        if (_tableRouteRules.TryGetValue(logicTable, out var ruleTypeClass))
        {
            throw new ShardingInvalidOperationException(
                $"table:{logicTable},already use table route rule:[{ruleTypeClass}]");
        }

        _tableRouteRules.TryAdd(logicTable, routeRuleTypeClass);
    }

    public bool HasTableRouteRule(string logicTable)
    {
        return _tableRouteRules.ContainsKey(logicTable);
    }

    public string GetTableRouteRule(string logicTable)
    {
        if (!_tableRouteRules.TryGetValue(logicTable, out var routeRule))
        {
            throw new ShardingInvalidOperationException(
                $"table:{logicTable},table route rule not found");
        }

        return routeRule;
    }

    public IReadOnlyDictionary<string, string> GetDataSourceRouteRules()
    {
        return _dataSourceRouteRules.ToImmutableDictionary();
    }

    public IReadOnlyDictionary<string, string> GetTableRouteRules()
    {
        return _tableRouteRules.ToImmutableDictionary();
    }
}