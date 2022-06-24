using DotNetty.Transport.Channels;
using ShardingConnector.Protocol;
using ShardingConnector.Protocol.MySql.Constant;
using ShardingConnector.Proxy.Common;
using ShardingConnector.Proxy.Network.Packets.Generic;
using ShardingConnector.Proxy.Network.Packets.Handshakes;
using ShardingConnector.Proxy.Network.Servers;

namespace ShardingConnector.Proxy.Network.Authentications;

public class MySqlAuthenticationEngine:IAuthenticationEngine
{
    private readonly MySqlAuthenticationHandler _authenticationHandler;

    private MySqlConnectionPhaseEnum _connectionPhase = MySqlConnectionPhaseEnum.INITIAL_HANDSHAKE;
    private int _sequenceId;
    private string _username;
    private byte[] _authResponse;
    private string? _database;
    public MySqlAuthenticationEngine()
    {
        _authenticationHandler = new MySqlAuthenticationHandler();
        
    }

    public void Handshake(IChannelHandlerContext context, ServerConnection serverConnection)
    {
        int connectionId = ConnectionIdGenerator.GetInstance().NextId();
        serverConnection.ConnectionId = connectionId;
        _connectionPhase = MySqlConnectionPhaseEnum.AUTH_PHASE_FAST_PATH;
        context.WriteAndFlushAsync(new MySqlHandshakePacket(connectionId, _authenticationHandler.AuthPluginData));
    }

    public bool Auth(IChannelHandlerContext context, IPacketPayload payload, ServerConnection serverConnection)
    {
        if (MySqlConnectionPhaseEnum.AUTH_PHASE_FAST_PATH == _connectionPhase)
        {
            var response41 = new MySqlHandshakeResponse41Packet((MySqlPacketPayload)payload);
            _username = response41.Username;
            _authResponse = response41.AuthResponse;
            _database = response41.Database;
            _sequenceId = response41.GetSequenceId();
            //TODO
            // if (!string.IsNullOrEmpty(_database))
            // {
            //     
            // }

            if (0 != (response41.CapabilityFlags & (int)MySqlCapabilityFlagEnum.CLIENT_PLUGIN_AUTH)
                &&MySQLAuthenticationMethodProvider.SECURE_PASSWORD_AUTHENTICATION!=response41.AuthPluginName)
            {
                _connectionPhase = MySqlConnectionPhaseEnum.AUTHENTICATION_METHOD_MISMATCH;
                Console.WriteLine("不支持当前认证请求请重新发起");
                //发送请求认证方式切换的包
                // context.WriteAndFlushAsync()
                return false;
            }
        }else if (MySqlConnectionPhaseEnum.AUTHENTICATION_METHOD_MISMATCH == _connectionPhase)
        {
            Console.WriteLine("已重新切换认证方式");
        }

        var sqlErrorCode = _authenticationHandler.Login(_username,_authResponse,_database);
        if (sqlErrorCode != null)
        {
            //mysql error
            // context.WriteAndFlushAsync()
        }
        else
        {
            serverConnection.SetCurrentSchema(_database!);
            serverConnection.UserName = _username;
            context.WriteAndFlushAsync(new MySqlOkPacket(++_sequenceId));
        }

        return true;
    }
}