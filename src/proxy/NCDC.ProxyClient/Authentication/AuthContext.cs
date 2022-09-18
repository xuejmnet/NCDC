namespace NCDC.ProxyClient.Authentication;

public sealed class AuthContext
{
    public string? Username { get; }
    public string? Hostname { get; }
    public string? Database { get; }
    public bool Finished { get; }

    public AuthContext(string? username,string? hostname,string? database,bool finished)
    {
        Username = username;
        Hostname = hostname;
        Database = database;
        Finished = finished;
    }
    public static AuthContext Default=new 
}