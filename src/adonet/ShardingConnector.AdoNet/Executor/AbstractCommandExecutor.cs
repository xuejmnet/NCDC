using ShardingConnector.AdoNet.AdoNet.Core.Connection;
using ShardingConnector.AdoNet.AdoNet.Core.Context;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.Executor.Engine;
using ShardingConnector.Extensions;
using ShardingConnector.ParserBinder.Command;
using ShardingConnector.ShardingExecute.Execute;
using ShardingConnector.ShardingExecute.Prepare;
using ShardingConnector.Spi.DataBase.DataBaseType;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace ShardingConnector.AdoNet.Executor
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/16 00:00:00
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    /// <summary>
    /// 
    /// </summary>
    public class AbstractCommandExecutor
    {

        public IDatabaseType DatabaseType { get; }

        public ShardingConnection Connection { get; }

        public SqlExecutePrepareTemplate SqlExecutePrepareTemplate { get; }

        public SqlExecuteTemplate SqlExecuteTemplate { get; }

        public readonly ICollection<DbConnection> Connections = new LinkedList<DbConnection>();

        public readonly List<List<object>> ParameterSets = new List<List<object>>();

        public readonly List<DbCommand> Statements = new List<DbCommand>();

        /// <summary>
        /// 并发?
        /// </summary>
        public readonly List<DbDataReader> ResultSets = new List<DbDataReader>();

        public readonly ICollection<InputGroup<CommandExecuteUnit>> InputGroups = new LinkedList<InputGroup<CommandExecuteUnit>>();

        public ISqlCommandContext<ISqlCommand> SqlStatementContext { get; set; }

        public AbstractCommandExecutor(ShardingConnection shardingConnection)
        {
            this.DatabaseType = shardingConnection.GetRuntimeContext().GetDatabaseType();
            this.Connection = shardingConnection;
            // int maxConnectionsSizePerQuery = connection.GetRuntimeContext().GetProperties().<Integer>getValue(ConfigurationPropertyKey.MAX_CONNECTIONS_SIZE_PER_QUERY);
            int maxConnectionsSizePerQuery = 1;
            ExecutorEngine executorEngine = Connection.GetRuntimeContext().GetExecutorEngine();
            SqlExecutePrepareTemplate = new SqlExecutePrepareTemplate(maxConnectionsSizePerQuery);
            SqlExecuteTemplate = new SqlExecuteTemplate(executorEngine, Connection.IsHoldTransaction());
        }

        /**
             * Clear data.
             *
             * @throws SQLException SQL exception
             */
        public void Clear()
        {
            ClearStatements();
            Statements.Clear();
            ParameterSets.Clear();
            Connections.Clear();
            ResultSets.Clear();
            InputGroups.Clear();
        }

        private void ClearStatements()
        {
            foreach (var dbCommand in Statements)
            {
                dbCommand.Dispose();
            }
        }
        protected void CacheCommands()
        {
            foreach (var inputGroup in InputGroups)
            {
                Statements.AddAll(inputGroup.Inputs.Select(o => o.Command).ToList());
                ParameterSets.AddAll(inputGroup.Inputs.Select(o => o.ExecutionUnit.GetSqlUnit().GetParameters()).ToList());
            }
        }

        /**
         * To make sure SkyWalking will be available at the next release of ShardingSphere,
         * a new plugin should be provided to SkyWalking project if this API changed.
         * 
         * @see <a href="https://github.com/apache/skywalking/blob/master/docs/en/guides/Java-Plugin-Development-Guide.md#user-content-plugin-development-guide">Plugin Development Guide</a>
         * 
         * @param executeCallback execute callback
         * @param <T> class type of return value 
         * @return result
         * @throws SQLException SQL exception
         */
        protected List<T> ExecuteCallback<T>(SqlExecuteCallback<T> executeCallback)
        {
            List<T> result = SqlExecuteTemplate.Execute(InputGroups, executeCallback);
            refreshMetaDataIfNeeded(Connection.GetRuntimeContext(), SqlStatementContext);
            return result;
        }

        /**
         * is accumulate.
         * 
         * @return accumulate or not
         */
        public bool IsAccumulate()
        {
            return false;
            // return !connection.GetRuntimeContext().GetRule().isAllBroadcastTables(SqlCommandContext.GetTablesContext().GetTableNames());
        }


        private void refreshMetaDataIfNeeded(ShardingRuntimeContext runtimeContext, ISqlCommandContext<ISqlCommand> sqlStatementContext)
        {
            if (null == sqlStatementContext)
            {
                return;
            }
            // if (sqlStatementContext instanceof CreateTableStatementContext) {
            //     refreshTableMetaData(runtimeContext, ((CreateTableStatementContext) sqlStatementContext).getSqlStatement());
            // } else if (sqlStatementContext instanceof AlterTableStatementContext) {
            //     refreshTableMetaData(runtimeContext, ((AlterTableStatementContext) sqlStatementContext).getSqlStatement());
            // } else if (sqlStatementContext instanceof DropTableStatementContext) {
            //     refreshTableMetaData(runtimeContext, ((DropTableStatementContext) sqlStatementContext).getSqlStatement());
            // } else if (sqlStatementContext instanceof CreateIndexStatementContext) {
            //     refreshTableMetaData(runtimeContext, ((CreateIndexStatementContext) sqlStatementContext).getSqlStatement());
            // } else if (sqlStatementContext instanceof DropIndexStatementContext) {
            //     refreshTableMetaData(runtimeContext, ((DropIndexStatementContext) sqlStatementContext).getSqlStatement());
            // }
        }

        // private void refreshTableMetaData(ShardingRuntimeContext runtimeContext, CreateTableStatement createTableStatement) {
        //     String tableName = createTableStatement.getTable().getTableName().getIdentifier().getValue();
        //     runtimeContext.getMetaData().getSchema().put(tableName, loadTableMeta(tableName, databaseType));
        // }

        // private void refreshTableMetaData(ShardingRuntimeContext runtimeContext, AlterTableStatement alterTableStatement) {
        //     String tableName = alterTableStatement.getTable().getTableName().getIdentifier().getValue();
        //     runtimeContext.getMetaData().getSchema().put(tableName, loadTableMeta(tableName, databaseType));
        // }

        // private void refreshTableMetaData(ShardingRuntimeContext runtimeContext, DropTableStatement dropTableStatement) {
        //     for (SimpleTableSegment each : dropTableStatement.getTables()) {
        //         runtimeContext.getMetaData().getSchema().remove(each.getTableName().getIdentifier().getValue());
        //     }
        // }
        //
        // private void refreshTableMetaData(ShardingRuntimeContext runtimeContext, CreateIndexStatement createIndexStatement) {
        //     if (null == createIndexStatement.getIndex()) {
        //         return;
        //     }
        //     String indexName = createIndexStatement.getIndex().getIdentifier().getValue();
        //     runtimeContext.getMetaData().getSchema().get(createIndexStatement.getTable().getTableName().getIdentifier().getValue()).getIndexes().put(indexName, new IndexMetaData(indexName));
        // }
        //
        // private void refreshTableMetaData(ShardingRuntimeContext runtimeContext, DropIndexStatement dropIndexStatement) {
        //     Collection<String> indexNames = getIndexNames(dropIndexStatement);
        //     TableMetaData tableMetaData = runtimeContext.getMetaData().getSchema().get(dropIndexStatement.getTable().getTableName().getIdentifier().getValue());
        //     if (null != dropIndexStatement.getTable()) {
        //         for (String each : indexNames) {
        //             tableMetaData.getIndexes().remove(each);
        //         }
        //     }
        //     for (String each : indexNames) {
        //         if (findLogicTableName(runtimeContext.getMetaData().getSchema(), each).isPresent()) {
        //             tableMetaData.getIndexes().remove(each);
        //         }
        //     }
        // }
        //
        // private Collection<String> getIndexNames(DropIndexStatement dropIndexStatement) {
        //     Collection<String> result = new LinkedList<>();
        //     for (IndexSegment each : dropIndexStatement.getIndexes()) {
        //         result.add(each.getIdentifier().getValue());
        //     }
        //     return result;
        // }
        //
        // private Optional<String> findLogicTableName(SchemaMetaData schemaMetaData,String logicIndexName) {
        //     for (String each : schemaMetaData.getAllTableNames()) {
        //         if (schemaMetaData.get(each).getIndexes().containsKey(logicIndexName)) {
        //             return Optional.of(each);
        //         }
        //     }
        //     return Optional.empty();
        // }
        //
        // private TableMetaData loadTableMeta(String tableName, IDatabaseType databaseType) {
        //     ShardingRule shardingRule = connection.getRuntimeContext().getRule();
        //     int maxConnectionsSizePerQuery = connection.getRuntimeContext().getProperties().<Integer>getValue(ConfigurationPropertyKey.MAX_CONNECTIONS_SIZE_PER_QUERY);
        //     boolean isCheckingMetaData = connection.getRuntimeContext().getProperties().<Boolean>getValue(ConfigurationPropertyKey.CHECK_TABLE_METADATA_ENABLED);
        //     TableMetaData result = new ShardingMetaDataLoader(connection.getDataSourceMap(), shardingRule, maxConnectionsSizePerQuery, isCheckingMetaData).load(tableName, databaseType);
        //     result = new ShardingTableMetaDataDecorator().decorate(result, tableName, shardingRule);
        //     if (!shardingRule.getEncryptRule().getEncryptTableNames().isEmpty()) {
        //         result = new EncryptTableMetaDataDecorator().decorate(result, tableName, shardingRule.getEncryptRule());
        //     }
        //     return result;
        // }
    }
}