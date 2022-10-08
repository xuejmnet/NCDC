using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using NCDC.Basic.User;
using NCDC.Exceptions;
using NCDC.ProxyServer.Contexts;

namespace NCDC.ProxyServer.AppServices;

public sealed class DefaultAppAppRuntimeManager:IAppRuntimeManager,IAppRuntimeLoader,IAppUserLoader,IUserDatabaseMappingLoader
{
    private readonly ConcurrentDictionary<string, IRuntimeContext> _runtimeContexts=new();
    private readonly ConcurrentDictionary<string, AuthUser> _authUsers=new();
    private readonly ConcurrentDictionary<UserDatabaseEntry, object?> _userDatabases=new();
    public bool LoadRuntimeContext(IRuntimeContext runtimeContext)
    {
        if (_runtimeContexts.ContainsKey(runtimeContext.DatabaseName))
        {
            return false;
        }

        return _runtimeContexts.TryAdd(runtimeContext.DatabaseName, runtimeContext);
    }

    public bool UnLoadRemoveRuntimeContext(string databaseName, [MaybeNullWhen(false)] out IRuntimeContext runtimeContext)
    {
       
        if (!_runtimeContexts.ContainsKey(databaseName))
        {
            runtimeContext = default;
            return false;
        }

        return _runtimeContexts.Remove(databaseName, out runtimeContext);
    }

    public IRuntimeContext GetRuntimeContext(string databaseName)
    {
        if (!_runtimeContexts.TryGetValue(databaseName, out var runtimeContext))
        {
            throw new ShardingException($"cant get runtime context:{databaseName}");
        }

        return runtimeContext;
    }

    public bool ContainsRuntimeContext(string databaseName)
    {
        return _runtimeContexts.ContainsKey(databaseName);
    }

    public ICollection<string> GetAllDatabaseNames()
    {
        return _runtimeContexts.Keys;
    }

    public ICollection<string> GetAllUsers()
    {
        return _authUsers.Keys;
    }

    public ICollection<string> GetAuthorizedDatabases(string username)
    {
        return _userDatabases.Keys.Where(o => o.UserName == username).Select(o=>o.Database).ToList();
    }

    public ICollection<string> GetAuthorizedUsers(string database)
    {
        return _userDatabases.Keys.Where(o => o.Database == database).Select(o=>o.UserName).ToList();
    }

    public AuthUser GetUser(string username)
    {
        if (!_authUsers.TryGetValue(username, out var u))
        {
            throw new ShardingException($"cant found user:{username}");
        }
        return u;
    }

    public bool AddAppUser(AuthUser authUser)
    {
        if (ContainsAppUser(authUser.Grantee.Username))
        {
            return false;
        }

        return _authUsers.TryAdd(authUser.Grantee.Username, authUser);
    }

    public bool RemoveAppUser(string username)
    {
        if (_authUsers.TryRemove(username, out _))
        {
            var authorizedDatabases = GetAuthorizedDatabases(username);
            foreach (var authorizedDatabase in authorizedDatabases)
            {
                _userDatabases.TryRemove(new UserDatabaseEntry(username,authorizedDatabase), out _);
            }
            return true;
        }

        return false;
    }

    public bool ContainsAppUser(string username)
    {
        return _authUsers.ContainsKey(username);
    }

    public bool AddUserDatabaseMapping(UserDatabaseEntry entry)
    {
        if (ContainsUserDatabaseMapping(entry))
        {
            return false;
        }

        return _userDatabases.TryAdd(entry, null);
    }

    public bool RemoveUserDatabaseMapping(UserDatabaseEntry entry)
    {
       
        if (!ContainsUserDatabaseMapping(entry))
        {
            return false;
        }

        return _userDatabases.TryRemove(entry, out _);
    }

    public bool ContainsUserDatabaseMapping(UserDatabaseEntry entry)
    {
        return _userDatabases.ContainsKey(entry);
    }
}