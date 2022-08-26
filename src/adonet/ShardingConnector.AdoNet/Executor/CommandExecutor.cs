// using ShardingConnector.AdoNet.AdoNet.Core.Connection;
// using ShardingConnector.Executor;
// using ShardingConnector.Executor.Constant;
// using ShardingConnector.ShardingExecute.Execute;
// using ShardingConnector.ShardingExecute.Execute.DataReader;
// using System;
// using System.Collections.Generic;
// using System.Data.Common;
// using System.Linq;
// using System.Threading;
// using System.Threading.Tasks;
// using ShardingConnector.Executor.Context;
// using ShardingConnector.Executor.Engine;
// using ShardingConnector.Extensions;
//
// namespace ShardingConnector.AdoNet.Executor
// {
//     /*
//     * @Author: xjm
//     * @Description:
//     * @Date: Friday, 16 April 2021 21:47:47
//     * @Email: 326308290@qq.com
//     */
//     public class CommandExecutor : AbstractCommandExecutor
//     {
//         public CommandExecutor(ShardingConnection shardingConnection) : base(shardingConnection)
//         {
//         }
//
//
//         public void Init(StreamMergeContext streamMergeContext)
//         {
//             SqlCommandContext = streamMergeContext.GetSqlCommandContext();
//             InputGroups.AddAll(GetExecuteGroups(streamMergeContext.GetExecutionUnits()));
//             CacheCommands();
//         }
//
//         private ICollection<InputGroup<CommandExecuteUnit>> GetExecuteGroups(ICollection<ExecutionUnit> executionUnits)
//         {
//             return SqlExecutePrepareTemplate.GetExecuteUnitGroups(executionUnits,
//                 new CommandGetGroupsSqlExecutePrepareCallback(this));
//         }
//         /**
//      * Execute query.
//      * 
//      * @return result set list
//      * @throws SQLException SQL exception
//      */
//         public List<IStreamDataReader> ExecuteQuery()
//         {
//             var isExceptionThrown = ExecutorExceptionHandler.IsThrowException();
//             SqlExecuteCallback<IStreamDataReader> executeCallback = new SqlExecuteCallback<IStreamDataReader>(DatabaseType, isExceptionThrown);
//             executeCallback.OnSqlExecute += GetQueryEnumerator;
//             return ExecuteCallback(executeCallback);
//         }
//         private IStreamDataReader GetQueryEnumerator(string sql, DbCommand command, ConnectionModeEnum connectionMode)
//         {
//             // DbDataReader resultSet = command.ExecuteReader(sql);
//             // command.CommandText = sql;
//             DbDataReader resultSet = command.ExecuteReader();
//             DbDataReaders.Add(resultSet);
//             if (ConnectionModeEnum.MEMORY_STRICTLY == connectionMode)
//                 return new StreamQueryDataReader(resultSet);
//             return new MemoryQueryDataReader(resultSet);
//         }
//         public int ExecuteNonQuery()
//         {
//             bool isExceptionThrown = ExecutorExceptionHandler.IsThrowException();
//             var executeCallback = new SqlExecuteCallback<int>(DatabaseType, isExceptionThrown);
//             executeCallback.OnSqlExecute += DoExecuteNonQuery;
//             var callback = ExecuteCallback(executeCallback);
//             return callback.Sum();
//         }
//         private int DoExecuteNonQuery(string sql, DbCommand command, ConnectionModeEnum connectionMode)
//         {
//             // DbDataReader resultSet = command.ExecuteReader(sql);
//             // command.CommandText = sql;
//             var i = command.ExecuteNonQuery();
//             return i;
//         }
//     }
// }