namespace ShardingConnector.Protocol.MySql.Constant;

/// <summary>
/// https://dev.mysql.com/doc/internals/en/authentication-method.html
/// </summary>
public class MySqlAuthenticationMethod
{
    public static string OLD_PASSWORD_AUTHENTICATION = "mysql_old_password";
    public static string NATIVE_PASSWORD_AUTHENTICATION = "mysql_native_password";
    public static string CLEAR_PASSWORD_AUTHENTICATION = "mysql_clear_password";
    public static string WINDOWS_CLIENT_AUTHENTICATION = "authentication_windows_client";
    public static string SHA256_AUTHENTICATION = "sha256_password";
}