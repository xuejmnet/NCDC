using System.Collections.Immutable;
using DotNetty.Transport.Channels;
using NCDC.Basic.User;
using NCDC.ProxyServer.AppServices;
using NCDC.ProxyServer.AppServices.Abstractions;
using NCDC.ProxyServer.Databases;
using NCDC.ProxyServer.Runtimes;

namespace NCDC.ProxyServer.Connection.Abstractions;

public interface IConnectionSession:IDisposable
{
    string? DatabaseName { get; }
    IServerConnection ServerConnection { get; }
    IChannel Channel { get; }
    // ILogicDatabase? LogicDatabase { get; }
    IVirtualDataSource? VirtualDataSource { get; }
    IRuntimeContext? RuntimeContext { get; }
    IAppRuntimeManager AppRuntimeManager { get; }
    ICollection<string> GetAllDatabaseNames();
    ICollection<string> GetAuthorizeDatabases();
    
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