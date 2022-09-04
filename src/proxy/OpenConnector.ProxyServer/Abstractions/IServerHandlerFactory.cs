using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParser.Command;
using OpenConnector.Common;
using OpenConnector.Configuration.Session;
using OpenConnector.Enums;
using OpenConnector.ProxyServer.Session;

namespace OpenConnector.ProxyServer.Abstractions;

public interface IServerHandlerFactory
{
    IServerHandler Create(DatabaseTypeEnum databaseType, string sql, ISqlCommand sqlCommand,
        ConnectionSession connectionSession);
}