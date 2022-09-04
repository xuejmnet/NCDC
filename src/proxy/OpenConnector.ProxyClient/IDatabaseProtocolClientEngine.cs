using OpenConnector.Configuration.Session;
using OpenConnector.ProxyClient.Abstractions;
using OpenConnector.ProxyClient.Authentication;
using OpenConnector.ProxyClient.Codecs;
using OpenConnector.ProxyClient.Command;
using OpenConnector.ProxyServer.Session;

namespace OpenConnector.ProxyClient;

public interface IDatabaseProtocolClientEngine
{
    
    IPacketCodec GetPacketCodec();
    IAuthenticationEngine GetAuthenticationEngine();
    IClientDbConnection GetClientDbConnection();
    void Release(ConnectionSession connectionSession);
    void HandleException(ConnectionSession connectionSession, Exception exception);
}