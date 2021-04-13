using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShardingConnector.Common.Rule;
using ShardingConnector.Extensions;
using ShardingConnector.Kernels.Route.Context;

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
        private readonly ICollection<ICollection<DataNode>> _originalDataNodes = new LinkedList<ICollection<DataNode>>();

        private readonly ISet<RouteUnit> _routeUnits = new HashSet<RouteUnit>();

        /**
         * Judge is route for single database and table only or not.
         *
         * @return is route for single database and table only or not
         */
        public bool IsSingleRouting()
        {
            return 1 == _routeUnits.Count;
        }

        /**
         * Get actual data source names.
         *
         * @return actual data source names
         */
        public ICollection<string> GetActualDataSourceNames()
        {
            return _routeUnits.Select(o => o.DataSourceMapper.ActualName).ToHashSet();
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
        public List<ISet<string>> GetActualTableNameGroups(string actualDataSourceName, ISet<string> logicTableNames)
        {
            return logicTableNames.Select(o => GetActualTableNames(actualDataSourceName, o))
                .Where(actualTableNames => actualTableNames.Any()).ToList();
        }

        private ISet<string> GetActualTableNames(string actualDataSourceName,string logicTableName)
        {
            ISet<string> result = new HashSet<string>();
            foreach (var routeUnit in _routeUnits)
            {
                if (actualDataSourceName.EqualsIgnoreCase(routeUnit.DataSourceMapper.ActualName))
                {
                    result.AddAll(routeUnit.GetActualTableNames(logicTableName));
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
        public IDictionary<string, ISet<string>> GetDataSourceLogicTablesMap(ICollection<string> actualDataSourceNames)
        {
            IDictionary<string, ISet<string>> result = new Dictionary<string, ISet<string>>();
            foreach (var actualDataSourceName in actualDataSourceNames)
            {

                ISet<string> logicTableNames = GetLogicTableNames(actualDataSourceName);
                if (logicTableNames.Any())
                {
                    result.Add(actualDataSourceName, logicTableNames);
                }
            }
            return result;
        }

        private ISet<string> GetLogicTableNames(string actualDataSourceName)
        {
            ISet<string> result = new HashSet<string>();
            foreach (var routeUnit in _routeUnits)
            {
                if (actualDataSourceName.EqualsIgnoreCase(routeUnit.DataSourceMapper.ActualName))
                {
                    result.AddAll(routeUnit.GetLogicTableNames());
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
        public RouteMapper FindTableMapper(string logicDataSourceName, string actualTableName)
        {
            foreach (var routeUnit in _routeUnits)
            {

                var result = routeUnit.FindTableMapper(logicDataSourceName, actualTableName);
                if (result!=null)
                {
                    return result;
                }
            }
            return null;
        }

        public ISet<RouteUnit> GetRouteUnits()
        {
            return _routeUnits;
        }

        public ICollection<ICollection<DataNode>> GetOriginalDataNodes()
        {
            return _originalDataNodes;
        }
    }
}
