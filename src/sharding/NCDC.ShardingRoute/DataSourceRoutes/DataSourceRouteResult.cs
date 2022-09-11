namespace NCDC.ShardingRoute.DataSourceRoutes;

public class DataSourceRouteResult
{
    public DataSourceRouteResult(ICollection<string> intersectDataSources)
    {
        IntersectDataSources = intersectDataSources;
    }
    public DataSourceRouteResult(string dataSource):this(new List<string>(){dataSource})
    {
    }
    /// <summary>
    /// 交集
    /// </summary>
    public ICollection<string> IntersectDataSources { get; }

    public override string ToString()
    {
        return $"data source route result:{string.Join(",", IntersectDataSources)}";
    }
}