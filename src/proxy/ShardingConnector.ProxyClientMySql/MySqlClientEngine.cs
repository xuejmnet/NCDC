using ShardingConnector.ProxyClient;
using ShardingConnector.ProxyClient.Abstractions;
using ShardingConnector.ProxyClient.Authentication;
using ShardingConnector.ProxyClient.Codecs;
using ShardingConnector.ProxyClient.Command;
using ShardingConnector.ProxyClientMySql.Authentication;
using ShardingConnector.ProxyServer.Session;
using MySqlPacketCodecEngine = ShardingConnector.ProxyClientMySql.Codec.MySqlPacketCodecEngine;

namespace ShardingConnector.ProxyClientMySql;

public class MySqlClientEngine:IDatabaseProtocolClientEngine
{
    private readonly IClientDbConnection _clientDbConnection;
    private readonly IPacketCodec _packetCodec;
    private readonly IAuthenticationEngine _authenticationEngine = new MySqlAuthenticationEngine();

    public MySqlClientEngine(IClientDbConnection clientDbConnection,IPacketCodec packetCodec)
    {
        _clientDbConnection = clientDbConnection;
        _packetCodec = packetCodec;
    }
    public IPacketCodec GetPacketCodec()
    {
        return _packetCodec;
    }

    public IAuthenticationEngine GetAuthenticationEngine()
    {
        return _authenticationEngine;
    }

    public IClientDbConnection GetClientDbConnection()
    {
        return _clientDbConnection;
    }

    public void Release(ConnectionSession connectionSession)
    {
        throw new NotImplementedException();
    }

    public void HandleException(ConnectionSession connectionSession, Exception exception)
    {
        Console.WriteLine(exception);
    }
}