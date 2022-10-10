// using System.Data.Common;
// using NCDC.ProxyServer.Connection.Abstractions;
//
// namespace NCDC.ProxyServer.Connection;
//
// public sealed class ServerDbDataReader:IServerDbDataReader
// {
//     private readonly IServerDbConnection _serverDbConnection;
//     private readonly DbDataReader _dataReader;
//
//     public ServerDbDataReader(IServerDbConnection serverDbConnection,DbDataReader dataReader)
//     {
//         _serverDbConnection = serverDbConnection;
//         _dataReader = dataReader;
//     }
//     public void Dispose()
//     {
//         _dataReader.Dispose();
//         _serverDbConnection.ServerDbDataReader = null;
//     }
//
//     public DbDataReader GetDbDataReader()
//     {
//         return _dataReader;
//     }
//
//     public IServerDbConnection GetServerDbConnection()
//     {
//         return _serverDbConnection;
//     }
// }