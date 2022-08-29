using OpenConnector.ProxyClient.Authentication;
using OpenConnector.ShardingCommon.User;

namespace OpenConnector.ProxyClientMySql.Authentication.Authenticator;

public interface IMySqlAuthenticator:IAuthenticator
{
    bool Authenticate(OpenConnectorUser user, byte[] authResponse);
}