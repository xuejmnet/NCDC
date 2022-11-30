using System.Text;
using NCDC.Basic.User;
using NCDC.Protocol.MySql.Constant;

namespace NCDC.ProxyClientMySql.Authentication.Authenticator;

public class MySqlClearPasswordAuthenticator:IMySqlAuthenticator
{
    public string GetAuthenticationMethodName()
    {
        return MySqlAuthenticationMethod.CLEAR_PASSWORD_AUTHENTICATION;
    }

    public bool Authenticate(AuthUser user, byte[] authResponse)
    {
        byte[] password = new byte[authResponse.Length - 1];
        Array.Copy(authResponse, 0, password, 0, authResponse.Length - 1);
        return string.IsNullOrEmpty(user.Password) || user.Password.Equals(Encoding.UTF8.GetString(password));
    }
}