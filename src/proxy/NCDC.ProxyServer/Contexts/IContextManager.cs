using System.Diagnostics.CodeAnalysis;
using NCDC.ProxyServer.Connection.User;

namespace NCDC.ProxyServer.Contexts;

public interface IContextManager
{
    bool AddRuntimeContext(IRuntimeContext runtimeContext);
    bool RemoveRuntimeContext(string databaseName,[MaybeNullWhen(false)] out IRuntimeContext runtimeContext);
    IRuntimeContext GetRuntimeContext(string databaseName);
    bool HasRuntimeContext(string databaseName);
    IReadOnlyCollection<string> GetAllDatabaseNames();
    IReadOnlyCollection<string> GetAllUsers();
    IReadOnlyCollection<string> GetAuthorizedDatabases(string username);
    IReadOnlyCollection<string> GetAuthorizedUsers(string database);
    AuthUser? TryGetUser(string username);
}