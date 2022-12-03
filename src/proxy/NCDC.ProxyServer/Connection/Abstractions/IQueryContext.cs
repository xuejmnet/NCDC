using NCDC.CommandParser.Common.Command;
using NCDC.ShardingAdoNet;
using NCDC.ShardingParser.Command;

namespace NCDC.ProxyServer.Connection.Abstractions;

public interface IQueryContext
{
     ISqlCommandContext<ISqlCommand> SqlCommandContext { get; }
     string Sql { get; }
     ParameterContext ParameterContext { get; }
     IConnectionSession ConnectionSession { get; }
     /// <summary>
     /// 是否是直接执行的sql
     /// </summary>
     bool DirectAllDataSourceSql { get; }
}