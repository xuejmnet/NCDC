// using NCDC.CommandParser.Common.Util;
// using NCDC.Protocol.MySql.Constant;
// using NCDC.Protocol.MySql.Packet.Generic;
// using NCDC.Protocol.MySql.Payload;
// using NCDC.Protocol.Packets;
// using NCDC.ProxyClient.Abstractions;
// using NCDC.ProxyClientMySql.Common;
// using NCDC.ProxyServer.Connection.Abstractions;
//
// namespace NCDC.ProxyClientMySql.ClientConnections.DataReaders.InitDb;
//
// public sealed class MySqlInitDbClientDataReader : IClientDataReader<MySqlPacketPayload>
// {
//     private readonly string _database;
//     private readonly IConnectionSession _connectionSession;
//
//     public MySqlInitDbClientDataReader(string database, IConnectionSession connectionSession)
//     {
//         _database = database;
//         _connectionSession = connectionSession;
//     }
//
//     public async IAsyncEnumerable<IPacket<MySqlPacketPayload>> SendCommand()
//     {
//         var exactlyDatabase = SqlUtil.GetExactlyValue(_database);
//         
//         if (_connectionSession.DatabaseExists(exactlyDatabase))
//         {
//             if (_connectionSession.GetAuthorizeDatabases().Contains(exactlyDatabase))
//             {
//                 _connectionSession.SetCurrentDatabaseName(exactlyDatabase);
//                 
//                
//                 var mySqlOkPacket = new MySqlOkPacket(1, ServerStatusFlagCalculator.CalculateFor(_connectionSession));
//                 yield return await Task.FromResult(mySqlOkPacket);
//             }
//            
//         }
//
//         else
//         {
//             var mySqlErrPacket = new MySqlErrPacket(1, MySqlServerErrorCode.ER_BAD_DB_ERROR_ARG1, _database);
//             yield return await Task.FromResult(mySqlErrPacket);
//         }
//     }
//
// }