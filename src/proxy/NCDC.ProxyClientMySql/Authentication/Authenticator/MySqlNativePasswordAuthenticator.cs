using NCDC.Basic.User;
using NCDC.Protocol.Helper;
using NCDC.Protocol.MySql.Constant;
using NCDC.Protocol.MySql.Packet.Handshake;

namespace NCDC.ProxyClientMySql.Authentication.Authenticator;

public class MySqlNativePasswordAuthenticator:IMySqlAuthenticator
{
    public MySqlAuthPluginData AuthPluginData { get; }

    public MySqlNativePasswordAuthenticator(MySqlAuthPluginData authPluginData)
    {
        AuthPluginData = authPluginData;
    }
    public string GetAuthenticationMethodName()
    {
        return MySqlAuthenticationMethod.NATIVE_PASSWORD_AUTHENTICATION;
    }

    public bool Authenticate(AuthUser user, byte[] authResponse)
    {
        return string.IsNullOrEmpty(user.Password) || GetAuthCipherBytes(user.Password).SequenceEqual(authResponse);
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