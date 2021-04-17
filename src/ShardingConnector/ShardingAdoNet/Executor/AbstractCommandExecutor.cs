using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using ShardingConnector.Execute;
using ShardingConnector.Executor.Engine;
using ShardingConnector.Extensions;
using ShardingConnector.Kernels.MetaData.Index;
using ShardingConnector.Kernels.MetaData.Table;
using ShardingConnector.Parser.Binder.Command;
using ShardingConnector.Parser.Sql.Command;
using ShardingConnector.Parser.Sql.Segment.Generic.Table;
using ShardingConnector.Prepare;
using ShardingConnector.Spi.DataBase.DataBaseType;

namespace ShardingConnector.ShardingAdoNet.Executor
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
        
    public IDatabaseType databaseType{ get;  }
    
    public int resultSetType{ get;  }
    
    public int resultSetConcurrency{ get;  }
    
    public int resultSetHoldability{ get;  }
    
    public ShardingConnection connection{ get;  }
    
    public SqlExecutePrepareTemplate sqlExecutePrepareTemplate{ get;  }
    
    public SqlExecuteTemplate sqlExecuteTemplate{ get;  }
    
    public readonly ICollection<DbConnection> connections = new LinkedList<DbConnection>();
    
    public readonly List<List<object>> parameterSets = new List<List<object>>();
    
    public readonly List<DbCommand> statements = new List<DbCommand>();
    
    /// <summary>
    /// 并发?
    /// </summary>
    public readonly List<DbDataReader> resultSets = new List<DbDataReader>();
    
    public readonly ICollection<InputGroup<CommandExecuteUnit>> inputGroups = new LinkedList<InputGroup<CommandExecuteUnit>>();
    
    public ISqlCommandContext<ISqlCommand> SqlStatementContext { get; set; }
    
    public AbstractCommandExecutor(int resultSetType, int resultSetConcurrency, int resultSetHoldability, ShardingConnection shardingConnection) {
        this.databaseType = shardingConnection.GetRuntimeContext().GetDatabaseType();
        this.resultSetType = resultSetType;
        this.resultSetConcurrency = resultSetConcurrency;
        this.resultSetHoldability = resultSetHoldability;
        this.connection = shardingConnection;
        // int maxConnectionsSizePerQuery = connection.GetRuntimeContext().GetProperties().<Integer>getValue(ConfigurationPropertyKey.MAX_CONNECTIONS_SIZE_PER_QUERY);
         int maxConnectionsSizePerQuery = 100;
        ExecutorEngine executorEngine = connection.GetRuntimeContext().GetExecutorEngine();
        sqlExecutePrepareTemplate = new SqlExecutePrepareTemplate(maxConnectionsSizePerQuery);
        sqlExecuteTemplate = new SqlExecuteTemplate(executorEngine, connection.IsHoldTransaction());
    }
    
    protected  void CacheStatements() {
        foreach (var inputGroup in inputGroups)
        {
            statements.AddAll(inputGroup.Inputs.Select(o=>o.Command).ToList());
            parameterSets.AddAll(inputGroup.Inputs.Select(o=>o.ExecutionUnit.GetSqlUnit().GetParameters()).ToList());
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
    protected  List<T> ExecuteCallback<T>(SqlExecuteCallback<T> executeCallback) {
        List<T> result = sqlExecuteTemplate.Execute(inputGroups, executeCallback);
        refreshMetaDataIfNeeded(connection.getRuntimeContext(), sqlStatementContext);
        return result;
    }
    
    /**
     * is accumulate.
     * 
     * @return accumulate or not
     */
    public  bool IsAccumulate() {
        return !connection.GetRuntimeContext().GetRule().isAllBroadcastTables(sqlStatementContext.getTablesContext().getTableNames());
    }
    
    /**
     * Clear data.
     *
     * @throws SQLException SQL exception
     */
    public void clear()  {
        clearStatements();
        statements.clear();
        parameterSets.clear();
        connections.clear();
        resultSets.clear();
        inputGroups.clear();
    }
    
    private void clearStatements() throws SQLException {
        for (Statement each : getStatements()) {
            each.close();
        }
    }
    
    private void refreshMetaDataIfNeeded( ShardingRuntimeContext runtimeContext,  SQLStatementContext sqlStatementContext) {
        if (null == sqlStatementContext) {
            return;
        }
        if (sqlStatementContext instanceof CreateTableStatementContext) {
            refreshTableMetaData(runtimeContext, ((CreateTableStatementContext) sqlStatementContext).getSqlStatement());
        } else if (sqlStatementContext instanceof AlterTableStatementContext) {
            refreshTableMetaData(runtimeContext, ((AlterTableStatementContext) sqlStatementContext).getSqlStatement());
        } else if (sqlStatementContext instanceof DropTableStatementContext) {
            refreshTableMetaData(runtimeContext, ((DropTableStatementContext) sqlStatementContext).getSqlStatement());
        } else if (sqlStatementContext instanceof CreateIndexStatementContext) {
            refreshTableMetaData(runtimeContext, ((CreateIndexStatementContext) sqlStatementContext).getSqlStatement());
        } else if (sqlStatementContext instanceof DropIndexStatementContext) {
            refreshTableMetaData(runtimeContext, ((DropIndexStatementContext) sqlStatementContext).getSqlStatement());
        }
    }
    
    private void refreshTableMetaData(final ShardingRuntimeContext runtimeContext, final CreateTableStatement createTableStatement) throws SQLException {
        String tableName = createTableStatement.getTable().getTableName().getIdentifier().getValue();
        runtimeContext.getMetaData().getSchema().put(tableName, loadTableMeta(tableName, databaseType));
    }
    
    private void refreshTableMetaData(final ShardingRuntimeContext runtimeContext, final AlterTableStatement alterTableStatement) throws SQLException {
        String tableName = alterTableStatement.getTable().getTableName().getIdentifier().getValue();
        runtimeContext.getMetaData().getSchema().put(tableName, loadTableMeta(tableName, databaseType));
    }
    
    private void refreshTableMetaData(final ShardingRuntimeContext runtimeContext, final DropTableStatement dropTableStatement) {
        for (SimpleTableSegment each : dropTableStatement.getTables()) {
            runtimeContext.getMetaData().getSchema().remove(each.getTableName().getIdentifier().getValue());
        }
    }
    
    private void refreshTableMetaData(final ShardingRuntimeContext runtimeContext, final CreateIndexStatement createIndexStatement) {
        if (null == createIndexStatement.getIndex()) {
            return;
        }
        String indexName = createIndexStatement.getIndex().getIdentifier().getValue();
        runtimeContext.getMetaData().getSchema().get(createIndexStatement.getTable().getTableName().getIdentifier().getValue()).getIndexes().put(indexName, new IndexMetaData(indexName));
    }
    
    private void refreshTableMetaData(final ShardingRuntimeContext runtimeContext, final DropIndexStatement dropIndexStatement) {
        Collection<String> indexNames = getIndexNames(dropIndexStatement);
        TableMetaData tableMetaData = runtimeContext.getMetaData().getSchema().get(dropIndexStatement.getTable().getTableName().getIdentifier().getValue());
        if (null != dropIndexStatement.getTable()) {
            for (String each : indexNames) {
                tableMetaData.getIndexes().remove(each);
            }
        }
        for (String each : indexNames) {
            if (findLogicTableName(runtimeContext.getMetaData().getSchema(), each).isPresent()) {
                tableMetaData.getIndexes().remove(each);
            }
        }
    }
    
    private Collection<String> getIndexNames(final DropIndexStatement dropIndexStatement) {
        Collection<String> result = new LinkedList<>();
        for (IndexSegment each : dropIndexStatement.getIndexes()) {
            result.add(each.getIdentifier().getValue());
        }
        return result;
    }
    
    private Optional<String> findLogicTableName(final SchemaMetaData schemaMetaData, final String logicIndexName) {
        for (String each : schemaMetaData.getAllTableNames()) {
            if (schemaMetaData.get(each).getIndexes().containsKey(logicIndexName)) {
                return Optional.of(each);
            }
        }
        return Optional.empty();
    }
    
    private TableMetaData loadTableMeta(final String tableName, final DatabaseType databaseType) throws SQLException {
        ShardingRule shardingRule = connection.getRuntimeContext().getRule();
        int maxConnectionsSizePerQuery = connection.getRuntimeContext().getProperties().<Integer>getValue(ConfigurationPropertyKey.MAX_CONNECTIONS_SIZE_PER_QUERY);
        boolean isCheckingMetaData = connection.getRuntimeContext().getProperties().<Boolean>getValue(ConfigurationPropertyKey.CHECK_TABLE_METADATA_ENABLED);
        TableMetaData result = new ShardingMetaDataLoader(connection.getDataSourceMap(), shardingRule, maxConnectionsSizePerQuery, isCheckingMetaData).load(tableName, databaseType);
        result = new ShardingTableMetaDataDecorator().decorate(result, tableName, shardingRule);
        if (!shardingRule.getEncryptRule().getEncryptTableNames().isEmpty()) {
            result = new EncryptTableMetaDataDecorator().decorate(result, tableName, shardingRule.getEncryptRule());
        }
        return result;
    }
    }
}