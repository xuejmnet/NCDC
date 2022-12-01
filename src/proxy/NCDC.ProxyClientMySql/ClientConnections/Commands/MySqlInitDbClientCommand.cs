// using NCDC.Protocol.MySql.Payload;
// using NCDC.ProxyClient.Abstractions;
// using NCDC.ProxyClientMySql.ClientConnections.DataReaders.InitDb;
// using NCDC.ProxyServer.Connection.Abstractions;
//
// namespace NCDC.ProxyClientMySql.ClientConnections.Commands;
//
// public sealed class MySqlInitDbClientCommand:IClientCommand<MySqlPacketPayload>
// {
//     private readonly IConnectionSession _connectionSession;
//     private readonly string _schema;
//     public MySqlInitDbClientCommand(MySqlPacketPayload payload,IConnectionSession connectionSession)
//     {
//         _connectionSession = connectionSession;
//         _schema = payload.ReadStringEOF();
//     }
//     public IClientDataReader<MySqlPacketPayload> ExecuteReader()
//     {
//         return new MySqlInitDbClientDataReader(_schema, _connectionSession);
//     }
// }