using ShardingConnector.Protocol.Core.Error;
using ShardingConnector.Protocol.Helper;
using ShardingConnector.Protocol.MySql.Constant;
using ShardingConnector.Proxy.Common.Context;
using ShardingConnector.Proxy.Network.Packets.Handshakes;
using ShardingConnector.ShardingCommon.Core.Rule;

namespace ShardingConnector.Proxy.Network.Authentications;

public class MySqlAuthenticationHandler
{
    private static readonly ShardingProxyContext SHARDING_PROXY_CONTEXT = ShardingProxyContext.GetInstance();

    public MySqlAuthPluginData AuthPluginData { get; }

    public MySqlAuthenticationHandler()
    {
        AuthPluginData = new MySqlAuthPluginData();
    }

    /// <summary>
    /// 用户登录
    /// </summary>
    /// <param name="username"></param>
    /// <param name="authResponse"></param>
    /// <param name="database"></param>
    /// <returns>返回null说明登录成功</returns>
    public ISqlErrorCode? Login(string username, byte[] authResponse, string? database)
    {
        var proxyUser = GetUser(username);
        if (proxyUser == null || !IsPasswordRight(proxyUser.Password, authResponse))
        {
            return MySqlServerErrorCodeProvider.GetInstance().ER_DBACCESS_DENIED_ERROR;
        }

        if (!IsAuthorizedSchema(proxyUser.AuthorizedSchemas, database))
        {
            return MySqlServerErrorCodeProvider.GetInstance().ER_DBACCESS_DENIED_ERROR;
        }

        return null;
    }

    private ProxyUser? GetUser(string username)
    {
        if (SHARDING_PROXY_CONTEXT.Authentication.Users.TryGetValue(username, out var proxyUser))
        {
            return proxyUser;
        }

        return null;
    }

    public bool IsPasswordRight(string password, byte[] authResponse)
    {
        var authCipherBytes = GetAuthCipherBytes(password);
        return string.IsNullOrEmpty(password) || authCipherBytes.SequenceEqual(authResponse);
    }

    private bool IsAuthorizedSchema(ICollection<string> authorizedSchemas, string? schema)
    {
        return string.IsNullOrEmpty(schema) || authorizedSchemas.Count == 0 || authorizedSchemas.Contains(schema);
    }
    public byte[] GetAuthCipherBytes(string password)
    {
        var sha1Password = Sha1Helper.Hash2Bytes(password);
        var doubleSha1Password = Sha1Helper.Hash2Bytes(sha1Password);
        var authPluginData = AuthPluginData.GetAuthPluginData();
        var concatBytes = new byte[authPluginData.Length+doubleSha1Password.Length];
        Array.Copy(authPluginData,0,concatBytes,0,authPluginData.Length);
        Array.Copy(doubleSha1Password,0,concatBytes,authPluginData.Length,doubleSha1Password.Length);
        var shar1ConcatBytes = Sha1Helper.Hash2Bytes(concatBytes);
        return XOR(sha1Password, shar1ConcatBytes);
    }

    public byte[] XOR(byte[] input,byte[] secret)
    {
        var result = new byte[input.Length];
        for (int i = 0; i < input.Length; i++)
        {
            result[i] = (byte)(input[i] ^ secret[i]);
        }

        return result;
    }
    
}