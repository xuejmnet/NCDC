using DotNetty.Common.Utilities;
using NCDC.Configuration.Metadatas;
using NCDC.Configuration.User;

namespace NCDC.Configuration.Connection.Abstractions;

public interface IConnectionSession
{
    IServerConnection ServerConnection { get; }
    IAttributeMap AttributeMap { get; }
    ILogicDatabase? LogicDatabase { get; }

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