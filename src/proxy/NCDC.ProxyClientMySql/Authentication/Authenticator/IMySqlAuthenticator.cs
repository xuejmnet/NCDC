
using NCDC.Basic.User;
using NCDC.ProxyClient.Authentication;

namespace NCDC.ProxyClientMySql.Authentication.Authenticator;

public interface IMySqlAuthenticator:IAuthenticator
{
    bool Authenticate(AuthUser user, byte[] authResponse);
}