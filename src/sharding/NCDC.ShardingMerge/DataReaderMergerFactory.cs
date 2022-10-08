using NCDC.Basic.Configurations;
using NCDC.Basic.TableMetadataManagers;
using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Command.DAL.Dialect;
using NCDC.ShardingMerge.Abstractions;
using NCDC.ShardingMerge.DataReaderMergers;
using NCDC.ShardingMerge.DataReaderMergers.DAL;
using NCDC.ShardingMerge.DataReaderMergers.DQL;
using NCDC.ShardingParser.Command;
using NCDC.ShardingParser.Command.DML;

namespace NCDC.ShardingMerge;

public sealed class DataReaderMergerFactory:IDataReaderMergerFactory
{
    private readonly ShardingConfiguration _shardingConfiguration;
    private readonly ITableMetadataManager _tableMetadataManager;

    public DataReaderMergerFactory(ShardingConfiguration shardingConfiguration,ITableMetadataManager tableMetadataManager)
    {
        _shardingConfiguration = shardingConfiguration;
        _tableMetadataManager = tableMetadataManager;
    }
    public IDataReaderMerger Create(ISqlCommandContext<ISqlCommand> sqlCommandContext)
    {
        if (sqlCommandContext is SelectCommandContext selectCommandContext)
        {
            return new ShardingDQLDataReaderMerger(_shardingConfiguration.DatabaseType, _tableMetadataManager);
        }

        if (sqlCommandContext.GetSqlCommand() is DALCommand dalCommand)
        {
            return new ShardingDALDataReaderMerger(_shardingConfiguration, _tableMetadataManager);
        }

        return new TransparentDataReaderMerger();
    }
}