using ShardingConnector.CommandParser.Command;
using ShardingConnector.Common;
using ShardingConnector.ProxyServer.Session;

namespace ShardingConnector.ProxyServer.Abstractions;

public interface ITextProtocolHandlerFactory
{
    ITextProtocolHandler Create(DatabaseTypeEnum databaseType, string sql, ISqlCommand sqlCommand,
        ConnectionSession connectionSession);
}