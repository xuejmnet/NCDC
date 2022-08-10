using ShardingConnector.ProtocolCore.Codecs;
using ShardingConnector.ProtocolMysql.Codec;
using ShardingConnector.ProxyClient;
using ShardingConnector.ProxyClient.Authentication;
using ShardingConnector.ProxyClientMySql.Authentication;

namespace ShardingConnector.ProxyClientMySql;

public class MySqlClientEngine:IDatabaseProtocolClientEngine
{
    private readonly IDatabasePacketCodecEngine _databasePacketCodecEngine = new MySqlPacketCodecEngine();
    private readonly IAuthenticationEngine _authenticationEngine = new MySqlAuthenticationEngine();
    public IDatabasePacketCodecEngine GetCodecEngine()
    {
        return _databasePacketCodecEngine;
    }

    public IAuthenticationEngine GetAuthenticationEngine()
    {
        return _authenticationEngine;
    }
}