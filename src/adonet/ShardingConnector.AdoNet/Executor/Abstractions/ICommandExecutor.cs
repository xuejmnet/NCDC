// using System.Collections.Generic;
// using System.Data.Common;
// using System.Threading;
// using System.Threading.Tasks;
// using ShardingConnector.Executor.Context;
//
// namespace ShardingConnector.AdoNet.Executor.Abstractions
// {
//     public interface ICommandExecutor
//     {
//         List<IStreamDataReader> ExecuteDbDataReader(bool serial, StreamMergeContext streamMergeContext, CancellationToken cancellationToken=new CancellationToken());
//         List<DbDataReader> GetDataReaders();
//         void Clear();
//         List<int> ExecuteNonQuery(bool serial,StreamMergeContext streamMergeContext, CancellationToken cancellationToken=new CancellationToken());
//     }
// }