using ShardingConnector.ProtocolCore.Codecs;
using ShardingConnector.ProxyClient.Authentication;

namespace ShardingConnector.ProxyClient;

public interface IDatabaseProtocolClientEngine
{
    IDatabasePacketCodecEngine GetCodecEngine();
    IAuthenticationEngine GetAuthenticationEngine();
}