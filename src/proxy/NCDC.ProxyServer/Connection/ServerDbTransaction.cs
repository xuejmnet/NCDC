// using System.Data;
// using System.Data.Common;
// using NCDC.ProxyServer.Connection.Abstractions;
//
// namespace NCDC.ProxyServer.Connection;
//
// public sealed class ServerDbTransaction:IServerDbTransaction
// {
//     private readonly IServerDbConnection _serverDbConnection;
//     private readonly DbTransaction _dbTransaction;
//
//     public ServerDbTransaction(IServerDbConnection serverDbConnection,DbTransaction dbTransaction)
//     {
//         _serverDbConnection = serverDbConnection;
//         _dbTransaction = dbTransaction;
//     }
//     public void Dispose()
//     {
//         _dbTransaction.Dispose();
//         _serverDbConnection.ServerDbTransaction = null;
//     }
//
//
//     public async ValueTask CommitTransactionAsync()
//     {
//       await  _dbTransaction.CommitAsync();
//     }
//
//     public async ValueTask RollbackTransactionAsync()
//     {
//         await _dbTransaction.RollbackAsync();
//     }
//
//     public IServerDbConnection GetServerDbConnection()
//     {
//         return _serverDbConnection;
//     }
// }