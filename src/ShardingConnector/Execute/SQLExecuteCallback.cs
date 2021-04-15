using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using ShardingConnector.Executor.Engine;
using ShardingConnector.Kernels.Parse;
using ShardingConnector.Spi.DataBase.DataBaseType;
using ShardingConnector.Spi.DataBase.MetaData;

namespace ShardingConnector.Execute
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/15 16:36:42
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public abstract class SQLExecuteCallback<T>:IGroupedCallback<CommandExecuteUnit,T>
    {
        private static readonly ConcurrentDictionary<string, IDataSourceMetaData> CACHED_DATASOURCE_METADATA = new ConcurrentDictionary<string, IDataSourceMetaData>();
    
        private readonly IDatabaseType databaseType;
    
        private readonly bool isExceptionThrown;

        protected SQLExecuteCallback(IDatabaseType databaseType, bool isExceptionThrown)
        {
            this.databaseType = databaseType;
            this.isExceptionThrown = isExceptionThrown;
        }

        public ICollection<T> Execute(ICollection<CommandExecuteUnit> inputs, bool isTrunkThread, IDictionary<string, object> dataMap)
        { ICollection<T> result = new LinkedList<T>();
            foreach (var input in inputs)
            {
                result.Add(Execute0(input, isTrunkThread, dataMap));
            }
            return result;
        }
    /**
     * To make sure SkyWalking will be available at the next release of ShardingSphere,
     * a new plugin should be provided to SkyWalking project if this API changed.
     *
     * @see <a href="https://github.com/apache/skywalking/blob/master/docs/en/guides/Java-Plugin-Development-Guide.md#user-content-plugin-development-guide">Plugin Development Guide</a>
     * 
     * @param statementExecuteUnit statement execute unit
     * @param isTrunkThread is trunk thread 
     * @param dataMap data map
     * @return result
     * @throws SQLException SQL exception
     */
    private T Execute0(CommandExecuteUnit commandExecuteUnit, bool isTrunkThread, IDictionary<string, object> dataMap){
        ExecutorExceptionHandler.SetExceptionThrow(isExceptionThrown);
        IDataSourceMetaData dataSourceMetaData = getDataSourceMetaData(commandExecuteUnit.Command);
        SQLExecutionHook sqlExecutionHook = new SPISQLExecutionHook();
        try {
            ExecutionUnit executionUnit = statementExecuteUnit.getExecutionUnit();
            sqlExecutionHook.start(executionUnit.getDataSourceName(), executionUnit.getSqlUnit().getSql(), executionUnit.getSqlUnit().getParameters(), dataSourceMetaData, isTrunkThread, dataMap);
            T result = executeSQL(executionUnit.getSqlUnit().getSql(), statementExecuteUnit.getStatement(), statementExecuteUnit.getConnectionMode());
            sqlExecutionHook.finishSuccess();
            return result;
        } catch (final SQLException ex) {
            sqlExecutionHook.finishFailure(ex);
            ExecutorExceptionHandler.handleException(ex);
            return null;
        }
    }
    
    private IDataSourceMetaData GetDataSourceMetaData(DbCommand dbCommand) {
        string url = dbCommand.Connection.ConnectionString;
        if (CACHED_DATASOURCE_METADATA.ContainsKey(url)) {
            return CACHED_DATASOURCE_METADATA[url];
        }
        IDataSourceMetaData result = databaseType.GetDataSourceMetaData(url, null);
        CACHED_DATASOURCE_METADATA.put(url, result);
        return result;
    }
    
    protected abstract T executeSQL(String sql, Statement statement, ConnectionMode connectionMode) throws SQLException;
    }
}