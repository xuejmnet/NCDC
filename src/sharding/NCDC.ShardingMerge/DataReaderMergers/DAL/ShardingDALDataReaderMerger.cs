using NCDC.Basic.Configurations;
using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Common.Command;
using NCDC.ShardingMerge.Abstractions;
using NCDC.ShardingMerge.DataReaderMergers.DAL.Common;
using NCDC.ShardingMerge.DataReaderMergers.DAL.Show;
using NCDC.ShardingMerge.DataReaders.Transparent;
using NCDC.ShardingParser.Command;
using NCDC.ShardingParser.MetaData;
using NCDC.StreamDataReaders;

namespace NCDC.ShardingMerge.DataReaderMergers.DAL
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/5/5 10:24:07
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ShardingDALDataReaderMerger:IDataReaderMerger
    {
        private readonly ShardingConfiguration _shardingConfiguration;
        private readonly ITableMetadataManager _tableMetadataManager;

        public ShardingDALDataReaderMerger(ShardingConfiguration shardingConfiguration,ITableMetadataManager tableMetadataManager)
        {
            _shardingConfiguration = shardingConfiguration;
            _tableMetadataManager = tableMetadataManager;
        }

        public IStreamDataReader Merge(List<IStreamDataReader> streamDataReaders, ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            var dalStatement = sqlCommandContext.GetSqlCommand();
            // if (dalStatement is ShowDatabasesCommand showDatabasesCommand) {
            //     return new SingleLocalDataMergedDataReader(new List<object>(){ _shardingConfiguration.DatabaseName });
            // }
            // if (dalStatement is ShowTablesCommand || dalStatement is ShowTableStatusCommand || dalStatement is ShowIndexCommand) {
            //     return new LogicTablesMergedDataReader(_tableMetadataManager,sqlCommandContext, streamDataReaders);
            // }
            // if (dalStatement is ShowCreateTableCommand) {
            //     return new ShowCreateTableMergedDataReader(_tableMetadataManager, sqlCommandContext, streamDataReaders);
            // }
            return new TransparentMergedDataReader(streamDataReaders[0]);
        }
    }
}
