
using NCDC.ProxyClient.Authentication;
using NCDC.ProxyServer.Connection.User;

namespace NCDC.ProxyClientMySql.Authentication.Authenticator;

public interface IMySqlAuthenticator:IAuthenticator
{
    bool Authenticate(AuthUser user, byte[] authResponse);
}