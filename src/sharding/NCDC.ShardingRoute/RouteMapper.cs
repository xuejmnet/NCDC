namespace NCDC.ShardingRoute;


public sealed class RouteMapper
{
    public RouteMapper(string logicName, string actualName)
    {
        LogicName = logicName;
        ActualName = actualName;
    }
    /// <summary>
    /// 逻辑名称
    /// </summary>
    public  string LogicName { get; }
    /// <summary>
    /// 真实名称
    /// </summary>
    public  string ActualName { get; }
}