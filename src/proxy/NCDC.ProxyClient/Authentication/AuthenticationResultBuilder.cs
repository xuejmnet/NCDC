namespace NCDC.ProxyClient.Authentication;

public sealed class AuthenticationResultBuilder
{
    public static AuthenticationResult Finished(string username, string hostname, string? database)
    {
        return new AuthenticationResult(username, hostname, database, true);
    }

    public static AuthenticationResult Continued()
    {
        return new AuthenticationResult(null, null, null, false);
    }

    public static AuthenticationResult Continued(string username, string hostname, string? database)
    {
        return new AuthenticationResult(username, hostname, database, false);
    }
}