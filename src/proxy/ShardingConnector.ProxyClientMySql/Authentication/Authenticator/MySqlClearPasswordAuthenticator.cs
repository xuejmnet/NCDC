using System.Text;
using ShardingConnector.Protocol.MySql.Constant;
using ShardingConnector.ShardingCommon.User;

namespace ShardingConnector.ProxyClientMySql.Authentication.Authenticator;

public class MySqlClearPasswordAuthenticator:IMySqlAuthenticator
{
    public string GetAuthenticationMethodName()
    {
        return MySqlAuthenticationMethod.CLEAR_PASSWORD_AUTHENTICATION;
    }

    public bool Authenticate(ShardingConnectorUser user, byte[] authResponse)
    {
        byte[] password = new byte[authResponse.Length - 1];
        Array.Copy(authResponse, 0, password, 0, authResponse.Length - 1);
        return string.IsNullOrEmpty(user.Password) || user.Password.Equals(Encoding.Default.GetString(password));
    }
}