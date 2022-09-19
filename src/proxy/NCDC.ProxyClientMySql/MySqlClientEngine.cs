using NCDC.ProxyClient;
using NCDC.ProxyClient.Abstractions;
using NCDC.ProxyClient.Authentication;
using NCDC.ProxyClient.Codecs;
using NCDC.ProxyClientMySql.Authentication;
using NCDC.ProxyServer.Connection.Abstractions;
using NCDC.ProxyServer.Contexts;

namespace NCDC.ProxyClientMySql;

public class MySqlClientEngine:IDatabaseProtocolClientEngine
{
    private readonly IClientDbConnection _clientDbConnection;
    private readonly IPacketCodec _packetCodec;
    private readonly IAuthenticationHandler _authenticationHandler ;

    public MySqlClientEngine(IClientDbConnection clientDbConnection,IPacketCodec packetCodec,IContextManager contextManager)
    {
        _authenticationHandler = new MySqlAuthenticationHandler(contextManager);
        _clientDbConnection = clientDbConnection;
        _packetCodec = packetCodec;
    }
    public IPacketCodec GetPacketCodec()
    {
        return _packetCodec;
    }

    public IAuthenticationHandler GetAuthenticationHandler()
    {
        return _authenticationHandler;
    }

    public IAuthContext GetAuthContext()
    {
        return new MySqlAuthContext();
    }
    //
    // public IAuthenticationEngine GetAuthenticationEngine()
    // {
    //     return _authenticationEngine;
    // }
    

    public IClientDbConnection GetClientDbConnection()
    {
        return _clientDbConnection;
    }

    public void Release(IConnectionSession connectionSession)
    {
        throw new NotImplementedException();
    }

    public void HandleException(IConnectionSession connectionSession, Exception exception)
    {
        Console.WriteLine(exception);
    }
}