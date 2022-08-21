using ShardingConnector.ProtocolCore.Errors;
using ShardingConnector.ProtocolMysql.Constant;
using ShardingConnector.ProtocolMysql.Helper;
using ShardingConnector.ProtocolMysql.Packet.Handshake;
using ShardingConnector.ProxyClientMySql.Authentication.Authenticator;
using ShardingConnector.ShardingCommon.Core.Rule;
using ShardingConnector.ShardingCommon.User;

namespace ShardingConnector.ProxyClientMySql.Authentication;

public class MySqlAuthenticationHandler
{

    public MySqlAuthPluginData AuthPluginData = new MySqlAuthPluginData();


    /// <summary>
    /// 用户登录
    /// </summary>
    /// <param name="username"></param>
    /// <param name="hostname"></param>
    /// <param name="authResponse"></param>
    /// <param name="databaseName"></param>
    /// <returns>返回null说明登录成功</returns>
    public ISqlErrorCode? Login(string username,string hostname, byte[] authResponse, string? databaseName)
    {
        var grantee = new Grantee(username,hostname);
        var mySqlAuthenticator = GetAuthenticator(username,hostname);
        if (!mySqlAuthenticator.Authenticate(new ShardingConnectorUser(grantee.Username, "123456", grantee.Hostname), authResponse))
        {
            return MySqlServerErrorCode.ER_ACCESS_DENIED_ERROR_ARG3;
        }

        var databaseHasPerm = true;
        return (null==databaseName || databaseHasPerm)?null:MySqlServerErrorCode.ER_ACCESS_DENIED_ERROR_ARG3;
        // var proxyUser = GetUser(username);
        // if (proxyUser == null || !IsPasswordRight(proxyUser.Password, authResponse))
        // {
        //     return MySqlServerErrorCodeProvider.GetInstance().ER_DBACCESS_DENIED_ERROR;
        // }
        //
        // if (!IsAuthorizedSchema(proxyUser.AuthorizedSchemas, database))
        // {
        //     return MySqlServerErrorCodeProvider.GetInstance().ER_DBACCESS_DENIED_ERROR;
        // }
        //
        // return null;
    }

    // private ProxyUser? GetUser(string username)
    // {
    //     if (SHARDING_PROXY_CONTEXT.Authentication.Users.TryGetValue(username, out var proxyUser))
    //     {
    //         return proxyUser;
    //     }
    //
    //     return null;
    // }

    // public bool IsPasswordRight(string password, byte[] authResponse)
    // {
    //     var authCipherBytes = GetAuthCipherBytes(password);
    //     return string.IsNullOrEmpty(password) || authCipherBytes.SequenceEqual(authResponse);
    // }
    //
    // private bool IsAuthorizedSchema(ICollection<string> authorizedSchemas, string? schema)
    // {
    //     return string.IsNullOrEmpty(schema) || authorizedSchemas.Count == 0 || authorizedSchemas.Contains(schema);
    // }

    public IMySqlAuthenticator GetAuthenticator(string username, string hostname)
    {
        return new MySqlNativePasswordAuthenticator(AuthPluginData);
    }
    
}