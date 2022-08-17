using ShardingConnector.ProtocolCore.Codecs;
using ShardingConnector.ProxyClient.Authentication;
using ShardingConnector.ProxyClient.Command;
using ShardingConnector.ProxyServer.Session;

namespace ShardingConnector.ProxyClient;

public interface IDatabaseProtocolClientEngine
{
    
    IDatabasePacketCodecEngine GetCodecEngine();
    IAuthenticationEngine GetAuthenticationEngine();
    ICommandExecuteEngine GetCommandExecuteEngine();
    void Release(ConnectionSession connectionSession);
    void HandleException(ConnectionSession connectionSession, Exception exception);
}