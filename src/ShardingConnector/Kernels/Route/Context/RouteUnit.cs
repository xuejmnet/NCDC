using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShardingConnector.Extensions;

namespace ShardingConnector.Kernels.Route.Context
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/13 16:14:21
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class RouteUnit
    {
        public  RouteMapper DataSourceMapper{get;}

        public ICollection<RouteMapper> TableMappers { get; }

        public RouteUnit(RouteMapper dataSourceMapper, ICollection<RouteMapper> tableMappers)
        {
            DataSourceMapper = dataSourceMapper;
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
                if (logicDataSourceName.EqualsIgnoreCase(DataSourceMapper.LogicName) &&
                    actualTableName.EqualsIgnoreCase(tableMapper.ActualName))
                {
                    return tableMapper;
                }
            }
            return null;
        }
    }
}
