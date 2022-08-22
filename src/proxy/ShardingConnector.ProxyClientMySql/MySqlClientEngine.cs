using ShardingConnector.ProtocolCore.Codecs;
using ShardingConnector.ProtocolMysql.Codec;
using ShardingConnector.ProxyClient;
using ShardingConnector.ProxyClient.Abstractions;
using ShardingConnector.ProxyClient.Authentication;
using ShardingConnector.ProxyClient.Codecs;
using ShardingConnector.ProxyClient.Command;
using ShardingConnector.ProxyClientMySql.Authentication;
using ShardingConnector.ProxyClientMySql.Command;
using ShardingConnector.ProxyServer.Session;
using MySqlPacketCodecEngine = ShardingConnector.ProxyClientMySql.Codec.MySqlPacketCodecEngine;

namespace ShardingConnector.ProxyClientMySql;

public class MySqlClientEngine:IDatabaseProtocolClientEngine
{
    private readonly ICommandExecuteEngine _commandExecuteEngine;
    private readonly IPacketCodec _packetCodec;
    private readonly IAuthenticationEngine _authenticationEngine = new MySqlAuthenticationEngine();

    public MySqlClientEngine(ICommandExecuteEngine commandExecuteEngine,IPacketCodec packetCodec)
    {
        _commandExecuteEngine = commandExecuteEngine;
        _packetCodec = packetCodec;
    }
    public IPacketCodec GetCodecEngine()
    {
        return _packetCodec;
    }

    public IAuthenticationEngine GetAuthenticationEngine()
    {
        return _authenticationEngine;
    }

    public ICommandExecuteEngine GetCommandExecuteEngine()
    {
        return _commandExecuteEngine;
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