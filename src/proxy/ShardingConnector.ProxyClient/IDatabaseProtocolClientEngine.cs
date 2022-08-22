using ShardingConnector.ProtocolCore.Codecs;
using ShardingConnector.ProxyClient.Authentication;
using ShardingConnector.ProxyClient.Codecs;
using ShardingConnector.ProxyClient.Command;
using ShardingConnector.ProxyServer.Session;

namespace ShardingConnector.ProxyClient;

public interface IDatabaseProtocolClientEngine
{
    
    IPacketCodec GetCodecEngine();
    IAuthenticationEngine GetAuthenticationEngine();
    ICommandExecuteEngine GetCommandExecuteEngine();
    void Release(ConnectionSession connectionSession);
    void HandleException(ConnectionSession connectionSession, Exception exception);
}