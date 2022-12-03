using NCDC.Extensions;

namespace NCDC.ShardingRoute;

public sealed class RouteUnit
{
    public string DataSource { get; }

    public List<RouteMapper> TableMappers { get; }

    public RouteUnit(string dataSource, List<RouteMapper> tableMappers)
    {
        DataSource = dataSource;
        TableMappers = tableMappers;
    }

    /**
         * Get logic table names.
         *
         * @return  logic table names
         */
    public ISet<string> GetLogicTableNames()
    {
        return TableMappers.Select(o => o.LogicName).ToHashSet();
    }

    /**
         * Get actual table names.
         *
         * @param logicTableName logic table name
         * @return actual table names
         */
    public ISet<string> GetActualTableNames(string logicTableName)
    {
        return TableMappers.Where(o => logicTableName.EqualsIgnoreCase(o.LogicName))
            .Select(o => o.ActualName).ToHashSet();
    }

    /**
         * Find table mapper.
         *
         * @param logicDataSourceName logic data source name
         * @param actualTableName actual table name
         * @return table mapper
         */
    public RouteMapper FindTableMapper(string logicDataSourceName, string actualTableName)
    {
        foreach (var tableMapper in TableMappers)
        {
            if (logicDataSourceName.EqualsIgnoreCase(DataSource) &&
                actualTableName.EqualsIgnoreCase(tableMapper.ActualName))
            {
                return tableMapper;
            }
        }

        return null;
    }
}