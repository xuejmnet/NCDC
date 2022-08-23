using ShardingConnector.CommandParser.Command;
using ShardingConnector.Common;
using ShardingConnector.ProxyServer.Session;

namespace ShardingConnector.ProxyServer.Abstractions;

public interface IServerHandlerFactory
{
    IServerHandler Create(DatabaseTypeEnum databaseType, string sql, ISqlCommand sqlCommand,
        ConnectionSession connectionSession);
}