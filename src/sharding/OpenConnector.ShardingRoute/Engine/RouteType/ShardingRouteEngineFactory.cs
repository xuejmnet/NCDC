using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Command;
using NCDC.CommandParser.Command.DAL.Dialect;
using NCDC.CommandParser.Command.DAL.Dialect.MySql;
using NCDC.CommandParser.Command.DAL.Dialect.PostgreSql;
using NCDC.CommandParser.Command.DCL;
using NCDC.CommandParser.Command.DDL;
using NCDC.CommandParser.Command.DML;
using NCDC.CommandParser.Command.TCL;
using NCDC.Common.Config.Properties;
using NCDC.Common.MetaData;
using OpenConnector.Extensions;
using NCDC.CommandParserBinder.Command;
using OpenConnector.ShardingCommon.Core.Rule;
using OpenConnector.ShardingRoute.Engine.Condition;
using OpenConnector.ShardingRoute.Engine.RouteType.Broadcast;
using OpenConnector.ShardingRoute.Engine.RouteType.Complex;
using OpenConnector.ShardingRoute.Engine.RouteType.DefaultDB;
using OpenConnector.ShardingRoute.Engine.RouteType.Ignore;
using OpenConnector.ShardingRoute.Engine.RouteType.Standard;
using OpenConnector.ShardingRoute.Engine.RouteType.Unicast;

namespace OpenConnector.ShardingRoute.Engine.RouteType
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/29 12:51:51
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ShardingRouteEngineFactory
    {
        private ShardingRouteEngineFactory()
        {

        }
        public static IShardingRouteEngine NewInstance(ShardingRule shardingRule,
                                                 OpenConnectorMetaData metaData, ISqlCommandContext<ISqlCommand> sqlCommandContext,
                                                 ShardingConditions shardingConditions, ConfigurationProperties properties)
        {
            var sqlStatement = sqlCommandContext.GetSqlCommand();
            ICollection<String> tableNames = sqlCommandContext.GetTablesContext().GetTableNames();
            if (sqlStatement is TCLCommand)
            {
                return new ShardingDatabaseBroadcastRoutingEngine();
            }
            if (sqlStatement is DDLCommand)
            {
                return new ShardingTableBroadcastRoutingEngine(metaData.Schema, sqlCommandContext);
            }
            if (sqlStatement is DALCommand)
            {
                return GetDALRoutingEngine(shardingRule, sqlStatement, tableNames);
            }
            if (sqlStatement is DCLCommand)
            {
                return GetDCLRoutingEngine(sqlCommandContext, metaData);
            }
            if (shardingRule.IsAllInDefaultDataSource(tableNames))
            {
                return new ShardingDefaultDatabaseRoutingEngine(tableNames);
            }
            if (shardingRule.IsAllBroadcastTables(tableNames))
            {
                if (sqlStatement is SelectCommand)
                    return new ShardingUnicastRoutingEngine(tableNames);
                return new ShardingDatabaseBroadcastRoutingEngine();
            }
            if (sqlCommandContext.GetSqlCommand() is DMLCommand && tableNames.IsEmpty() && shardingRule.HasDefaultDataSourceName())
            {
                return new ShardingDefaultDatabaseRoutingEngine(tableNames);
            }
            if (sqlCommandContext.GetSqlCommand() is DMLCommand && shardingConditions.IsAlwaysFalse() || tableNames.IsEmpty() || !shardingRule.TableRuleExists(tableNames))
            {
                return new ShardingUnicastRoutingEngine(tableNames);
            }
            return GetShardingRoutingEngine(shardingRule, sqlCommandContext, shardingConditions, tableNames, properties);
        }

        private static IShardingRouteEngine GetDALRoutingEngine(ShardingRule shardingRule, ISqlCommand sqlCommand, ICollection<String> tableNames)
        {
            if (sqlCommand is UseCommand)
            {
                return new ShardingIgnoreRoutingEngine();
            }
            if (sqlCommand is SetCommand || sqlCommand is ResetParameterCommand || sqlCommand is ShowDatabasesCommand)
            {
                return new ShardingDatabaseBroadcastRoutingEngine();
            }
            if (tableNames.Any() && !shardingRule.TableRuleExists(tableNames) && shardingRule.HasDefaultDataSourceName())
            {
                return new ShardingDefaultDatabaseRoutingEngine(tableNames);
            }
            if (tableNames.Any())
            {
                return new ShardingUnicastRoutingEngine(tableNames);
            }
            return new ShardingDataSourceGroupBroadcastRoutingEngine();
        }

        private static IShardingRouteEngine GetDCLRoutingEngine(ISqlCommandContext<ISqlCommand> sqlCommandContext, OpenConnectorMetaData metaData)
        {
            if (IsDCLForSingleTable(sqlCommandContext))
            {
                return new ShardingTableBroadcastRoutingEngine(metaData.Schema, sqlCommandContext);
            }
            return new ShardingMasterInstanceBroadcastRoutingEngine(metaData.DataSources);
        }

        private static bool IsDCLForSingleTable(ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            if (sqlCommandContext is ITableAvailable tableAvailable)
            {
                return 1 == tableAvailable.GetAllTables().Count && !"*".Equals(tableAvailable.GetAllTables().First().GetTableName().GetIdentifier().GetValue());
            }
            return false;
        }

        private static IShardingRouteEngine GetShardingRoutingEngine(ShardingRule shardingRule, ISqlCommandContext<ISqlCommand> sqlCommandContext,
                                                                    ShardingConditions shardingConditions, ICollection<String> tableNames, ConfigurationProperties properties)
        {
            ICollection<String> shardingTableNames = shardingRule.GetShardingLogicTableNames(tableNames);
            if (1 == shardingTableNames.Count || shardingRule.IsAllBindingTables(shardingTableNames))
            {
                return new ShardingStandardRoutingEngine(shardingTableNames.First(), sqlCommandContext, shardingConditions, properties);
            }
            // TODO config for cartesian set
            return new ShardingComplexRoutingEngine(tableNames, sqlCommandContext, shardingConditions, properties);
        }
    }
}
