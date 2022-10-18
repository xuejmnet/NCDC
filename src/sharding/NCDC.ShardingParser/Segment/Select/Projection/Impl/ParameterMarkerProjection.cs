using NCDC.CommandParser.Common.Constant;

namespace NCDC.ShardingParser.Segment.Select.Projection.Impl;

public sealed class ParameterMarkerProjection:IProjection
{
    private readonly string? _alias;
    public int ParameterMarkerIndex { get; }
    public ParameterMarkerTypeEnum ParameterMarkerType { get; }

    public ParameterMarkerProjection(int parameterMarkerIndex,ParameterMarkerTypeEnum parameterMarkerType,string? alias)
    {
        _alias = alias;
        ParameterMarkerIndex = parameterMarkerIndex;
        ParameterMarkerType = parameterMarkerType;
    }
    public string GetExpression()
    {
        return ParameterMarkerIndex.ToString();
    }

    public string? GetAlias()
    {
        return _alias;
    }

    public string GetColumnLabel()
    {
        return GetAlias() ?? GetExpression();
    }

    public string GetExpressionWithAlias()
    {
        return GetExpression()+(null == _alias ? "" : " AS " + _alias);
    }
}