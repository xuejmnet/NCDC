namespace NCDC.ProxyServer.Connection.Metadatas;

/// <summary>
/// 代理用户
/// </summary>
public sealed class ProxyUser
{
    public string Username { get; }
    public string Password { get; }

    public ProxyUser(string username,string password)
    {
        Username = username;
        Password = password;
    }

    private bool Equals(ProxyUser other)
    {
        return Username == other.Username && Password == other.Password;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is ProxyUser other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Username, Password);
    }
}