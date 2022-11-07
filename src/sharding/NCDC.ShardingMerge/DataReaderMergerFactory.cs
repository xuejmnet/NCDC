using NCDC.Basic.Configurations;
using NCDC.CommandParser.Common.Command;
using NCDC.CommandParser.Common.Command.DAL;
using NCDC.ShardingMerge.Abstractions;
using NCDC.ShardingMerge.DataReaderMergers;
using NCDC.ShardingMerge.DataReaderMergers.DAL;
using NCDC.ShardingMerge.DataReaderMergers.DQL;
using NCDC.ShardingParser.Command;
using NCDC.ShardingParser.Command.DML;
using NCDC.ShardingParser.MetaData;

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

        if (sqlCommandContext.GetSqlCommand() is IDALCommand dalCommand)
        {
            return new ShardingDALDataReaderMerger(_shardingConfiguration, _tableMetadataManager);
        }

        return new TransparentDataReaderMerger();
    }
}