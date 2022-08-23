using ShardingConnector.ProxyClient.Abstractions;
using ShardingConnector.ProxyClient.Authentication;
using ShardingConnector.ProxyClient.Codecs;
using ShardingConnector.ProxyClient.Command;
using ShardingConnector.ProxyServer.Session;

namespace ShardingConnector.ProxyClient;

public interface IDatabaseProtocolClientEngine
{
    
    IPacketCodec GetPacketCodec();
    IAuthenticationEngine GetAuthenticationEngine();
    IClientDbConnection GetClientDbConnection();
    void Release(ConnectionSession connectionSession);
    void HandleException(ConnectionSession connectionSession, Exception exception);
}