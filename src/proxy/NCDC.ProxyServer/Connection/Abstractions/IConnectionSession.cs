using System.Collections.Immutable;
using System.Data;
using DotNetty.Transport.Channels;
using NCDC.Basic.User;
using NCDC.CommandParser.Abstractions;
using NCDC.ProxyServer.AppServices;
using NCDC.ProxyServer.AppServices.Abstractions;
using NCDC.ProxyServer.Databases;
using NCDC.ProxyServer.Runtimes;

namespace NCDC.ProxyServer.Connection.Abstractions;

public interface IConnectionSession:IDisposable
{
    IsolationLevel IsolationLevel { get; set; }
    string? DatabaseName { get; }
    ISqlCommandParser GetSqlCommandParser();
    IServerConnection ServerConnection { get; }
    IChannel Channel { get; }
    // ILogicDatabase? LogicDatabase { get; }
    IVirtualDataSource VirtualDataSource { get; }
    IRuntimeContext RuntimeContext { get; }
    ICollection<string> GetAllDatabaseNames();

    // bool DatabaseExists(string database);
    bool GetIsAutoCommit();

    TransactionStatus GetTransactionStatus();

    int GetConnectionId();

    //
    // Grantee GetGrantee();
    //
    // void SetGrantee(Grantee grantee);

    // void SetCurrentDatabaseName(string? databaseName);
    Task WaitChannelIsWritableAsync(CancellationToken cancellationToken = default);
    void NotifyChannelIsWritable();
    QueryContext? QueryContext { get; set; }

    void Reset();
}