using System;
using System.Collections.Generic;
using System.Text;
using NCDC.ShardingMerge.DataReaderMergers.DAL.Common;
using NCDC.ShardingMerge.DataReaderMergers.DAL.Show;
using OpenConnector.Api.Database;
using OpenConnector.CommandParser.Command;
using OpenConnector.CommandParser.Command.DAL.Dialect.MySql;
using OpenConnector.Merge.Engine.Merger;
using OpenConnector.Merge.Reader.Transparent;
using OpenConnector.CommandParserBinder.Command;
using OpenConnector.CommandParserBinder.MetaData.Schema;
using OpenConnector.ShardingCommon.Core.Rule;
using OpenConnector.StreamDataReaders;

namespace OpenConnector.ShardingMerge.DAL
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/5/5 10:24:07
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ShardingDALEnumeratorMerger:IDataReaderMerger
    {
        private readonly ShardingRule shardingRule;

        public ShardingDALEnumeratorMerger(ShardingRule shardingRule)
        {
            this.shardingRule = shardingRule;
        }

        public IStreamDataReader Merge(List<IStreamDataReader> streamDataReaders, ISqlCommandContext<ISqlCommand> sqlCommandContext, SchemaMetaData schemaMetaData)
        {
            var dalStatement = sqlCommandContext.GetSqlCommand();
            if (dalStatement is ShowDatabasesCommand showDatabasesCommand) {
                return new SingleLocalDataMergedDataReader(new List<object>(){ DefaultSchema.LOGIC_NAME });
            }
            if (dalStatement is ShowTablesCommand || dalStatement is ShowTableStatusCommand || dalStatement is ShowIndexCommand) {
                return new LogicTablesMergedDataReader(shardingRule, schemaMetaData,sqlCommandContext, streamDataReaders);
            }
            if (dalStatement is ShowCreateTableCommand) {
                return new ShowCreateTableMergedDataReader(shardingRule,schemaMetaData, sqlCommandContext, streamDataReaders);
            }
            return new TransparentMergedDataReader(streamDataReaders[0]);
        }
    }
}
