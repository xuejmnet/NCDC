using NCDC.Plugin;

namespace NCDC.ShardingRoute.TableRoutes;

public sealed class TableRouteResult
{
    public TableRouteResult(List<TableRouteUnit> replaceTables)
    {
        ReplaceTables = replaceTables.ToHashSet();
        IsEmpty = replaceTables.Count == 0;
    }
    public TableRouteResult(TableRouteUnit replaceTable):this(new List<TableRouteUnit>(){replaceTable})
    {
    }
        
    public ISet<TableRouteUnit> ReplaceTables { get; }

    public bool IsEmpty { get; }

    protected bool Equals(TableRouteResult other)
    {
        return Equals(ReplaceTables, other.ReplaceTables) && IsEmpty == other.IsEmpty;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((TableRouteResult)obj);
    }

    public override string ToString()
    {
        return $"(current table:[{string.Join(",", ReplaceTables.Select(o => $"{o.DataSourceName}.{o.LogicTableName}.{o.ActualTableName}"))}])";
    }


    public override int GetHashCode()
    {
        return HashCode.Combine(ReplaceTables, IsEmpty);
    }
}