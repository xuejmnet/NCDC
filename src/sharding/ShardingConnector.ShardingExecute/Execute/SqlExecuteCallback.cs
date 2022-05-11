using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using ShardingConnector.Executor.Constant;
using ShardingConnector.Executor.Context;
using ShardingConnector.Executor.Engine;
using ShardingConnector.Executor.Hook;
using ShardingConnector.Spi.DataBase.DataBaseType;
using ShardingConnector.Spi.DataBase.MetaData;

namespace ShardingConnector.ShardingExecute.Execute
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
    public sealed class SqlExecuteCallback<T> : IGroupedCallback<CommandExecuteUnit, T>
    {
        private static readonly ConcurrentDictionary<string, IDataSourceMetaData> CACHED_DATASOURCE_METADATA = new ConcurrentDictionary<string, IDataSourceMetaData>();

        private readonly IDatabaseType databaseType;

        private readonly bool isExceptionThrown;
        public event Func<string, DbCommand, ConnectionModeEnum, T> OnSqlExecute;
        public event Func<string, DbCommand, ConnectionModeEnum, Task<T>> OnSqlExecuteAsync;


        public SqlExecuteCallback(IDatabaseType databaseType, bool isExceptionThrown)
        {
            this.databaseType = databaseType;
            this.isExceptionThrown = isExceptionThrown;
        }

        public ICollection<T> Execute(ICollection<CommandExecuteUnit> inputs, bool isTrunkThread, IDictionary<string, object> dataMap)
        {
            ICollection<T> result = new LinkedList<T>();
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
        private T Execute0(CommandExecuteUnit commandExecuteUnit, bool isTrunkThread, IDictionary<string, object> dataMap)
        {
            ExecutorExceptionHandler.SetExceptionThrow(isExceptionThrown);
            IDataSourceMetaData dataSourceMetaData = GetDataSourceMetaData(commandExecuteUnit.Command);
            var hookManager = SqlExecutionHookManager.GetInstance();
            try
            {
                ExecutionUnit executionUnit = commandExecuteUnit.ExecutionUnit;
                hookManager.Start(executionUnit.GetDataSourceName(), executionUnit.GetSqlUnit().GetSql(), executionUnit.GetSqlUnit().GetParameters(), dataSourceMetaData, isTrunkThread, dataMap);
                T result = OnSqlExecute(executionUnit.GetSqlUnit().GetSql(), commandExecuteUnit.Command, commandExecuteUnit.ConnectionMode);
                hookManager.FinishSuccess();
                return result;
            }
            catch (Exception ex)
            {
                hookManager.FinishFailure(ex);
                ExecutorExceptionHandler.HandleException(ex);
                return default;
            }
        }

        private IDataSourceMetaData GetDataSourceMetaData(DbCommand dbCommand)
        {
            string url = dbCommand.Connection.ConnectionString;
            if (CACHED_DATASOURCE_METADATA.ContainsKey(url))
            {
                return CACHED_DATASOURCE_METADATA[url];
            }

            IDataSourceMetaData result = databaseType.GetDataSourceMetaData(url, null);
            CACHED_DATASOURCE_METADATA.TryAdd(url, result);
            return result;
        }
    }
}