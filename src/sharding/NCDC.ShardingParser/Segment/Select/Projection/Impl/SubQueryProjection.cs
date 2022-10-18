namespace NCDC.ShardingParser.Segment.Select.Projection.Impl;

public sealed class SubQueryProjection:IProjection
{
    private readonly string _expression;
    private readonly string? _alias;

    public SubQueryProjection(string expression,string? alias)
    {
        _expression = expression;
        _alias = alias;
    }
    public string GetExpression()
    {
        return _expression;
    }

    public string? GetAlias()
    {
        return _alias;
    }

    public string GetColumnLabel()
    {
        return GetAlias() ?? GetExpression();
    }
}