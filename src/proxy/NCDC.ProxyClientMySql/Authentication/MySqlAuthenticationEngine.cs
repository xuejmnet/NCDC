// using System.Net;
// using DotNetty.Transport.Channels;
// using NCDC.Protocol.Errors;
// using NCDC.Protocol.MySql.Constant;
// using NCDC.Protocol.MySql.Packet.Generic;
// using NCDC.Protocol.MySql.Packet.Handshake;
// using NCDC.Protocol.MySql.Payload;
// using NCDC.Protocol.Packets;
// using NCDC.ProxyClient;
// using NCDC.ProxyClient.Authentication;
// using NCDC.ProxyClientMySql.Common;
// using NCDC.ProxyServer;
// using NCDC.ProxyServer.Contexts;
//
// namespace NCDC.ProxyClientMySql.Authentication;
//
// public class MySqlAuthenticationEngine:IAuthenticationEngine
// {
//     private readonly IAppRuntimeManager _appRuntimeManager;
//     private static readonly MySqlStatusFlagEnum DEFAULT_STATUS_FLAG = MySqlStatusFlagEnum.SERVER_STATUS_AUTOCOMMIT;
//     private readonly MySqlAuthenticationHandler _authenticationHandler;
//
//     private MySqlConnectionPhaseEnum _connectionPhase = MySqlConnectionPhaseEnum.INITIAL_HANDSHAKE;
//     private int _sequenceId;
//     private string? _username;
//     private byte[]? _authResponse;
//     private string? _database;
//     private AuthenticationResult? _currentAuthResult;
//
//     public MySqlAuthenticationEngine(IAppRuntimeManager appRuntimeManager)
//     {
//         _appRuntimeManager = appRuntimeManager;
//         _authenticationHandler= new MySqlAuthenticationHandler1(appRuntimeManager);
//     }
//
//     public int Handshake(IChannelHandlerContext context)
//     {
//         int connectionId = ConnectionIdGenerator.Instance.NextId();
//         _connectionPhase = MySqlConnectionPhaseEnum.AUTH_PHASE_FAST_PATH;
//         context.WriteAndFlushAsync(new MySqlHandshakePacket(connectionId, _authenticationHandler.AuthPluginData));
//         return connectionId;
//     }
//
//     public AuthenticationResult Authenticate(IChannelHandlerContext context, IPacketPayload payload)
//     {
//         if (MySqlConnectionPhaseEnum.AUTH_PHASE_FAST_PATH == _connectionPhase)
//         {
//             _currentAuthResult = AuthPhaseFastPath(context, payload);
//             if (!_currentAuthResult.Finished)
//             {
//                 return _currentAuthResult;
//             }
//         }else if (MySqlConnectionPhaseEnum.AUTHENTICATION_METHOD_MISMATCH == _connectionPhase)
//         {
//             Console.WriteLine("已重新切换认证方式");
//         }
//
//         var hostAddress = GetHostAddress(context);
//         var sqlErrorCode = _authenticationHandler.Login(_currentAuthResult.Username,hostAddress,_authResponse,_database);
//         if (sqlErrorCode != null)
//         {
//             //mysql error
//             context.WriteAndFlushAsync(CreateErrorPacket(sqlErrorCode, context));
//         }
//         else
//         {
//             context.WriteAndFlushAsync(new MySqlOkPacket(++_sequenceId,DEFAULT_STATUS_FLAG));
//         }
//
//         return AuthenticationResultBuilder.Finished(_currentAuthResult.Username,hostAddress,_currentAuthResult.Database);
//     }
//     private MySqlErrPacket CreateErrorPacket(ISqlErrorCode errorCode, IChannelHandlerContext context) {
//         return MySqlServerErrorCode.ER_DBACCESS_DENIED_ERROR_ARG3 == errorCode
//             ? new MySqlErrPacket(++_sequenceId, MySqlServerErrorCode.ER_DBACCESS_DENIED_ERROR_ARG3, _currentAuthResult?.Username??string.Empty, GetHostAddress(context), _currentAuthResult?.Database??string.Empty)
//             : new MySqlErrPacket(++_sequenceId, MySqlServerErrorCode.ER_ACCESS_DENIED_ERROR_ARG3, _currentAuthResult?.Username??string.Empty, GetHostAddress(context), GetErrorMessage());
//     }
//
//     private string GetErrorMessage() {
//         return 0 == _authResponse!.Length ? "NO" : "YES";
//     }
//     private AuthenticationResult AuthPhaseFastPath(IChannelHandlerContext context, IPacketPayload payload)
//     {
//         var packet = new MySqlHandshakeResponse41Packet((MySqlPacketPayload)payload);
//         _authResponse = packet.AuthResponse;
//         _sequenceId = packet.SequenceId;
//         var mySqlCharacterSet = MySqlCharacterSet.FindById(packet.CharacterSet);
//         context.Channel.GetAttribute(CommonConstants.CHARSET_ATTRIBUTE_KEY).Set(mySqlCharacterSet.Charset);
//         context.Channel.GetAttribute(MySqlConstants.MYSQL_CHARACTER_SET_ATTRIBUTE_KEY).Set(mySqlCharacterSet);
//         //todo check database
//         //packet.Database
//         var hostAddress = GetHostAddress(context);
//         var authenticator = _authenticationHandler.GetAuthenticator(packet.Username, hostAddress);
//         if (IsClientPluginAuth(packet) && !authenticator.GetAuthenticationMethodName().Equals(packet.AuthPluginName)) {
//             Console.WriteLine("不支持当前认证请求请重新发起");
//             // _connectionPhase = MySqlConnectionPhaseEnum.AUTHENTICATION_METHOD_MISMATCH;
//             // context.WriteAndFlushAsync(new MySqlAuthSwitchRequestPacket(++sequenceId, authenticator.getAuthenticationMethodName(), authenticationHandler.getAuthPluginData()));
//             // return AuthenticationResultBuilder.continued(packet.getUsername(), getHostAddress(context), packet.getDatabase());
//             return AuthenticationResultBuilder.Continued(packet.Username, hostAddress, packet.Database);
//         }
//         return AuthenticationResultBuilder.Finished(packet.Username, hostAddress, packet.Database);
//
//     }
//     
//     private bool IsClientPluginAuth(MySqlHandshakeResponse41Packet packet)
//     {
//         return packet.CapabilityFlagsEnum.HasFlag(MySqlCapabilityFlagEnum.CLIENT_PLUGIN_AUTH);
//     }
//     private string GetHostAddress(IChannelHandlerContext context) {
//         //获取Ip
//         IPEndPoint iPEndPoint = (IPEndPoint)context.Channel.RemoteAddress;
//             
//         string addr = iPEndPoint.Address.ToString();
//
//         if (!string.IsNullOrWhiteSpace(addr)) {
//             int index = addr.LastIndexOf("/", StringComparison.Ordinal);
//             if (index >= 0) {
//                 return addr.Substring(index + 1);
//             }
//             return addr;
//         }
//
//         return string.Empty;
//     }
// }