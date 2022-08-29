using OpenConnector.ProxyClient;
using OpenConnector.ProxyClient.Abstractions;
using OpenConnector.ProxyClient.Authentication;
using OpenConnector.ProxyClient.Codecs;
using OpenConnector.ProxyClient.Command;
using OpenConnector.ProxyClientMySql.Authentication;
using OpenConnector.ProxyServer.Session;
using MySqlPacketCodecEngine = OpenConnector.ProxyClientMySql.Codec.MySqlPacketCodecEngine;

namespace OpenConnector.ProxyClientMySql;

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