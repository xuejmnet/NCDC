using ShardingConnector.ProxyClient.Authentication;
using ShardingConnector.ShardingCommon.User;

namespace ShardingConnector.ProxyClientMySql.Authentication.Authenticator;

public interface IMySqlAuthenticator:IAuthenticator
{
    bool Authenticate(ShardingConnectorUser user, byte[] authResponse);
}