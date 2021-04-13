using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.Common.Rule;

namespace ShardingConnector.Kernels.Route
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/3/30 12:55:35
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class RouteResult
    {
        private readonly ICollection<ICollection<DataNode>> originalDataNodes = new LinkedList<ICollection<DataNode>>();

        private readonly ICollection<RouteUnit> routeUnits = new HashSet<RouteUnit>();

        /**
         * Judge is route for single database and table only or not.
         *
         * @return is route for single database and table only or not
         */
        public boolean isSingleRouting()
        {
            return 1 == routeUnits.size();
        }

        /**
         * Get actual data source names.
         *
         * @return actual data source names
         */
        public ICollection<String> getActualDataSourceNames()
        {
            return routeUnits.stream().map(each->each.getDataSourceMapper().getActualName()).collect(Collectors.toCollection(()-> new HashSet<>(routeUnits.size(), 1)));
        }

        /**
         * Get actual tables groups.
         * 
         * <p>
         * Actual tables in same group are belong one logic name.
         * </p>
         *
         * @param actualDataSourceName actual data source name
         * @param logicTableNames logic table names
         * @return actual table groups
         */
        public List<Set<String>> getActualTableNameGroups(final String actualDataSourceName, final Set<String> logicTableNames)
        {
            return logicTableNames.stream().map(each->getActualTableNames(actualDataSourceName, each)).filter(actualTableNames-> !actualTableNames.isEmpty()).collect(Collectors.toList());
        }

        private Set<String> getActualTableNames(final String actualDataSourceName, final String logicTableName)
        {
            Set<String> result = new HashSet<>();
            for (RouteUnit each : routeUnits)
            {
                if (actualDataSourceName.equalsIgnoreCase(each.getDataSourceMapper().getActualName()))
                {
                    result.addAll(each.getActualTableNames(logicTableName));
                }
            }
            return result;
        }

        /**
         * Get map relationship between actual data source and logic tables.
         *
         * @param actualDataSourceNames actual data source names
         * @return  map relationship between data source and logic tables
         */
        public Map<String, Set<String>> getDataSourceLogicTablesMap(final Collection<String> actualDataSourceNames)
        {
            Map<String, Set<String>> result = new HashMap<>();
            for (System.String each : actualDataSourceNames)
            {
                Set<String> logicTableNames = getLogicTableNames(each);
                if (!logicTableNames.isEmpty())
                {
                    result.put(each, logicTableNames);
                }
            }
            return result;
        }

        private Set<String> getLogicTableNames(final String actualDataSourceName)
        {
            Set<String> result = new HashSet<>();
            for (RouteUnit each : routeUnits)
            {
                if (actualDataSourceName.equalsIgnoreCase(each.getDataSourceMapper().getActualName()))
                {
                    result.addAll(each.getLogicTableNames());
                }
            }
            return result;
        }

        /**
         * Find table mapper.
         *
         * @param logicDataSourceName logic data source name
         * @param actualTableName actual table name
         * @return table mapper
         */
        public Optional<RouteMapper> findTableMapper(final String logicDataSourceName, final String actualTableName)
        {
            for (RouteUnit each : routeUnits)
            {
                Optional<RouteMapper> result = each.findTableMapper(logicDataSourceName, actualTableName);
                if (result.isPresent())
                {
                    return result;
                }
            }
            return Optional.empty();
        }
    }
}
