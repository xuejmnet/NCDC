namespace NCDC.ProxyClient.Authentication;

public class AuthenticationResult
{
    public string? Username { get; }
    public string? Hostname { get; }
    public string? Database { get; }
    public bool Finished { get; }

    public AuthenticationResult(string? username,string? hostname,string? database,bool finished)
    {
        Username = username;
        Hostname = hostname;
        Database = database;
        Finished = finished;
    }
}