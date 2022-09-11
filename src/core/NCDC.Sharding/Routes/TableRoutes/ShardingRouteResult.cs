namespace NCDC.Sharding.Routes.TableRoutes;

public class ShardingRouteResult
{
    
    public ICollection<RouteUnit> RouteUnits { get; }
    public bool IsCrossDataSource { get; }
    public bool IsCrossTable { get; }
    public bool IsEmpty { get; }

    public ShardingRouteResult(ICollection<RouteUnit> routeUnits,bool isEmpty,bool isCrossDataSource,bool isCrossTable)
    {
        var routeUnitGroup = routeUnits.GroupBy(o=>o.DataSource);
        RouteUnits = routeUnits;
        var count = routeUnitGroup.Count();
        IsEmpty =isEmpty;
        IsCrossDataSource = isCrossDataSource;
        IsCrossTable = isCrossTable;
    }

    public override string ToString()
    {
        return string.Join(",",RouteUnits.Select(o => o.ToString()));
    }
}