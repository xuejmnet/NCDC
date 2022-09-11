namespace NCDC.Sharding.Routes.DataSourceRoutes;

public class DataSourceRouteResult
{
    public DataSourceRouteResult(ISet<string> intersectDataSources)
    {
        IntersectDataSources = intersectDataSources;
    }
    public DataSourceRouteResult(string dataSource):this(new HashSet<string>(){dataSource})
    {
    }
    /// <summary>
    /// 交集
    /// </summary>
    public ISet<string> IntersectDataSources { get; }

    public override string ToString()
    {
        return $"data source route result:{string.Join(",", IntersectDataSources)}";
    }
}