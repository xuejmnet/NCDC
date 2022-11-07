using NCDC.CommandParser.Common.Command;
using NCDC.ShardingAdoNet;
using NCDC.ShardingParser.Command;

namespace NCDC.ProxyServer.Abstractions;

public interface ISqlCommandContextFactory
{
    ISqlCommandContext<ISqlCommand> Create(string sql, ParameterContext parameterContext, ISqlCommand sqlCommand);
}