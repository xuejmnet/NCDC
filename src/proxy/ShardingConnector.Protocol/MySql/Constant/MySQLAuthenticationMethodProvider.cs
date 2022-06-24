namespace ShardingConnector.Protocol.MySql.Constant;

public class MySQLAuthenticationMethodProvider
{
    public const string OLD_PASSWORD_AUTHENTICATION = "mysql_old_password";
    public const string SECURE_PASSWORD_AUTHENTICATION = "mysql_native_password";
    public const string CLEAR_TEXT_AUTHENTICATION = "mysql_clear_password";
    public const string WINDOWS_NATIVE_AUTHENTICATION = "authentication_windows_client";
    public const string SHA256 = "sha256_password";
}