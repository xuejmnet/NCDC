// using System;
// using System.Collections.Generic;
// using System.Data.Common;
// using System.Linq;
// using System.Threading;
// using System.Threading.Tasks;
// using ShardingConnector.AdoNet.Executor.Abstractions;
// using ShardingConnector.Exceptions;
// using ShardingConnector.Executor.Constant;
// using ShardingConnector.Executor.Context;
// using ShardingConnector.Extensions;
//
// namespace ShardingConnector.AdoNet.Executor
// {
//     public class NoQueryExecutor:IExecutor<int>
//     {
//         public event Func<DbCommand, ConnectionModeEnum, int> OnCommandSqlExecute;
//         public List<int> Execute(DataSourceSqlExecutorUnit dataSourceSqlExecutorUnit)
//         {
//             return Execute0(dataSourceSqlExecutorUnit);
//         }
//
//         private List<int> Execute0(DataSourceSqlExecutorUnit dataSourceSqlExecutorUnit)
//         {
//             var executorGroups = dataSourceSqlExecutorUnit.SqlExecutorGroups;
//             var result = new List<int>(executorGroups.Sum(o=>o.Groups.Count()));
//             foreach (var executorGroup in executorGroups)
//             {
//                 var routeQueryResults =  GroupExecute(executorGroup.Groups);
//                 result.AddAll(routeQueryResults);
//             }
//
//             return result;
//         }
//
//         private  int[] GroupExecute(List<CommandExecuteUnit> commandExecuteUnits)
//         {
//             if (commandExecuteUnits.Count <= 0)
//             {
//                 return Array.Empty<int>();
//             }
//
//             if (commandExecuteUnits.Count == 1)
//             {
//                 return new int[1] { ExecuteCommandUnit(commandExecuteUnits[0]) };
//             }
//             else
//             {
//                 CancellationToken cancellationToken = new CancellationToken();
//                 var dataReaders = new List<int>(commandExecuteUnits.Count());
//                 var otherTasks = commandExecuteUnits.Skip(1)
//                     .Select(o => Task.Run(() => ExecuteCommandUnit(o), cancellationToken)).ToArray();
//                 var streamDataReader = ExecuteCommandUnit(commandExecuteUnits[0]);
//                 var streamDataReaders = Task.WhenAll(otherTasks).GetAwaiter().GetResult();
//                 dataReaders.Add(streamDataReader);
//                 dataReaders.AddAll(streamDataReaders);
//                 return dataReaders.ToArray();
//             }
//         }
//
//         private  int ExecuteCommandUnit(CommandExecuteUnit commandExecuteUnit)
//         {
//             if (OnCommandSqlExecute == null)
//             {
//                 throw new ShardingException("command sql execute not implement");
//             }
//             return OnCommandSqlExecute.Invoke(commandExecuteUnit.Command, commandExecuteUnit.ConnectionMode);
//         }
//     }
// }