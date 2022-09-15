using NCDC.ProxyClient.Abstractions;
using NCDC.ProxyClient.Authentication;
using NCDC.ProxyClient.Codecs;
using NCDC.ProxyServer.Connection.Abstractions;

namespace NCDC.ProxyClient;

public interface IDatabaseProtocolClientEngine
{
    
    IPacketCodec GetPacketCodec();
    IAuthenticationEngine GetAuthenticationEngine();
    IClientDbConnection GetClientDbConnection();
    void Release(IConnectionSession connectionSession);
    void HandleException(IConnectionSession connectionSession, Exception exception);
}