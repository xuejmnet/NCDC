using System.Collections.Concurrent;
using System.Collections.Immutable;
using NCDC.ProxyServer.Connection.User;

namespace NCDC.ProxyServer.Contexts;

public sealed class DefaultUserManager:IUserManager
{
    private readonly ConcurrentDictionary<string, AuthUser> _users = new();
    public bool AddUser(AuthUser user)
    {
        return _users.TryAdd(user.Grantee.Username, user);
    }

    public bool HasUser(string username)
    {
        return _users.ContainsKey(username);
    }

    public AuthUser GetUser(string username)
    {
        if (!_users.TryGetValue(username, out var authUser))
        {
            throw new InvalidOperationException($"cant get user:[{username}]");
        }

        return authUser;
    }

    public IReadOnlyList<string> GetUserNames()
    {
        return _users.Keys.ToImmutableList();
    }

    public bool Remove(string username)
    {
        return _users.TryRemove(username, out var _);
    }
}