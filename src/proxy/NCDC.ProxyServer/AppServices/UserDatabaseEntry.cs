namespace NCDC.ProxyServer.AppServices;

public class UserDatabaseEntry
{
    public UserDatabaseEntry(string userName, string database)
    {
        UserName = userName;
        Database = database;
    }

    public string UserName { get; }
    public string Database { get; }

    protected bool Equals(UserDatabaseEntry other)
    {
        return UserName == other.UserName && Database == other.Database;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((UserDatabaseEntry)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(UserName, Database);
    }
}