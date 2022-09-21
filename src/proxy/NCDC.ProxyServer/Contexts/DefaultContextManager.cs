using System.Collections.Concurrent;
using System.Collections.Immutable;
using NCDC.Exceptions;
using NCDC.ProxyServer.Connection.User;

namespace NCDC.ProxyServer.Contexts;

public sealed class DefaultContextManager:IContextManager
{
    private readonly ConcurrentDictionary<string, IRuntimeContext> _runtimeContexts=new();
    public bool AddRuntimeContext(string databaseName,IRuntimeContext runtimeContext)
    {
        throw new NotImplementedException();
    }

    public IRuntimeContext GetRuntimeContext(string databaseName)
    {
        if (!_runtimeContexts.TryGetValue(databaseName, out var runtimeContext))
        {
            throw new ShardingException($"cant get runtime context:{databaseName}");
        }

        return runtimeContext;
    }

    public bool HasRuntimeContext(string databaseName)
    {
        return _runtimeContexts.ContainsKey(databaseName);
    }

    public IReadOnlyCollection<string> GetAllDatabaseNames()
    {
        return _runtimeContexts.Keys.ToImmutableList();
    }

    public IReadOnlyCollection<string> GetAllUsers()
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<string> GetAuthorizedDatabases(string username)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<string> GetAuthorizedUsers(string database)
    {
        throw new NotImplementedException();
    }

    public AuthUser? TryGetUser(string username)
    {
        throw new NotImplementedException();
    }
}