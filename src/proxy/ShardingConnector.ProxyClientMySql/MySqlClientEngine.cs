using ShardingConnector.ProtocolCore.Codecs;
using ShardingConnector.ProtocolMysql.Codec;
using ShardingConnector.ProxyClient;
using ShardingConnector.ProxyClient.Abstractions;
using ShardingConnector.ProxyClient.Authentication;
using ShardingConnector.ProxyClient.Command;
using ShardingConnector.ProxyClientMySql.Authentication;
using ShardingConnector.ProxyClientMySql.Command;
using ShardingConnector.ProxyServer.Session;

namespace ShardingConnector.ProxyClientMySql;

public class MySqlClientEngine:IDatabaseProtocolClientEngine
{
    private readonly ICommandExecuteEngine _commandExecuteEngine;
    private readonly IDatabasePacketCodecEngine _databasePacketCodecEngine = new MySqlPacketCodecEngine();
    private readonly IAuthenticationEngine _authenticationEngine = new MySqlAuthenticationEngine();

    public MySqlClientEngine(ICommandExecuteEngine commandExecuteEngine)
    {
        _commandExecuteEngine = commandExecuteEngine;
    }
    public IDatabasePacketCodecEngine GetCodecEngine()
    {
        return _databasePacketCodecEngine;
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