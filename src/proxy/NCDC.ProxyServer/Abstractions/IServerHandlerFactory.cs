using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Common.Command;
using NCDC.Enums;
using NCDC.ProxyServer.Connection.Abstractions;

namespace NCDC.ProxyServer.Abstractions;

public interface IServerHandlerFactory
{
    IServerHandler Create(DatabaseTypeEnum databaseType, string sql, ISqlCommand sqlCommand,
        IConnectionSession connectionSession);
}