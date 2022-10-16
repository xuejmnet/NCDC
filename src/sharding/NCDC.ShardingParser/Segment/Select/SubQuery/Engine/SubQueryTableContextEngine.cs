using NCDC.ShardingParser.Command.DML;
using NCDC.ShardingParser.Segment.Select.Projection.Impl;

namespace NCDC.ShardingParser.Segment.Select.SubQuery.Engine;

public static class SubQueryTableContextEngine
{
    
    public static IEnumerable<SubQueryTableContext> CreateSubQueryTableContexts( SelectCommandContext subQueryContext,  string? alias) {
        List<String> columnNames = subQueryContext.GetProjectionsContext().GetExpandProjections()
            .Where(o => o is ColumnProjection)
            .Select(o => ((ColumnProjection)o).GetName()).ToList();
        foreach (var tableName in subQueryContext.GetTablesContext().GetTableNames())
        {
            yield return new SubQueryTableContext(tableName, alias, columnNames);
        }
    }
}