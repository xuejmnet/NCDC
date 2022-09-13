using NCDC.Basic.Metadatas;
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
    private readonly IDatabaseSettings _databaseSettings;
    private readonly ITableMetadataManager _tableMetadataManager;

    public DataReaderMergerFactory(IDatabaseSettings databaseSettings,ITableMetadataManager tableMetadataManager)
    {
        _databaseSettings = databaseSettings;
        _tableMetadataManager = tableMetadataManager;
    }
    public IDataReaderMerger Create(ISqlCommandContext<ISqlCommand> sqlCommandContext)
    {
        if (sqlCommandContext is SelectCommandContext selectCommandContext)
        {
            return new ShardingDQLDataReaderMerger(_databaseSettings.GetDatabaseType(), _tableMetadataManager);
        }

        if (sqlCommandContext.GetSqlCommand() is DALCommand dalCommand)
        {
            return new ShardingDALDataReaderMerger(_databaseSettings, _tableMetadataManager);
        }

        return new TransparentDataReaderMerger();
    }
}