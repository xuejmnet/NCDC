using System.Collections.Immutable;
using DotNetty.Common.Utilities;
using NCDC.Basic.Metadatas;
using NCDC.ProxyServer.Connection.Metadatas;
using NCDC.ProxyServer.Connection.User;
using NCDC.ProxyServer.Contexts;

namespace NCDC.ProxyServer.Connection.Abstractions;

public interface IConnectionSession
{
    string? DatabaseName { get; }
    IServerConnection ServerConnection { get; }
    IAttributeMap AttributeMap { get; }
    ILogicDatabase? LogicDatabase { get; }
    IRuntimeContext? RuntimeContext { get; }
    IRuntimeContextManager RuntimeContextManager { get; }
    IReadOnlyCollection<string> GetAllDatabaseNames();
    IReadOnlyCollection<string> GetAuthorizeDatabases();
    
    bool DatabaseExists(string database);
    bool GetIsAutoCommit();

    TransactionStatus GetTransactionStatus();

    int GetConnectionId();

    void SetConnectionId(int connectionId);

    Grantee GetGrantee();

    void SetGrantee(Grantee grantee);

    void SetCurrentDatabaseName(string? databaseName);
    Task WaitChannelIsWritableAsync(CancellationToken cancellationToken = default);
    void NotifyChannelIsWritable();

    void CloseServerConnection()
    {
        ServerConnection.CloseCurrentCommandReader();
    }
}