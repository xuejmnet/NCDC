using System.Text;
using OpenConnector.Configuration.User;
using OpenConnector.Protocol.MySql.Constant;

namespace OpenConnector.ProxyClientMySql.Authentication.Authenticator;

public class MySqlClearPasswordAuthenticator:IMySqlAuthenticator
{
    public string GetAuthenticationMethodName()
    {
        return MySqlAuthenticationMethod.CLEAR_PASSWORD_AUTHENTICATION;
    }

    public bool Authenticate(OpenConnectorUser user, byte[] authResponse)
    {
        byte[] password = new byte[authResponse.Length - 1];
        Array.Copy(authResponse, 0, password, 0, authResponse.Length - 1);
        return string.IsNullOrEmpty(user.Password) || user.Password.Equals(Encoding.Default.GetString(password));
    }
}