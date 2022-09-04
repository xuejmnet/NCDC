using OpenConnector.Configuration.User;
using OpenConnector.ProxyClient.Authentication;

namespace OpenConnector.ProxyClientMySql.Authentication.Authenticator;

public interface IMySqlAuthenticator:IAuthenticator
{
    bool Authenticate(OpenConnectorUser user, byte[] authResponse);
}