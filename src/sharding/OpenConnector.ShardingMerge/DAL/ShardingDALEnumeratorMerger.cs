using System;
using System.Collections.Generic;
using System.Text;

using OpenConnector.Api.Database;
using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Command;
using NCDC.CommandParser.Command.DAL.Dialect.MySql;
using OpenConnector.Merge.Engine.Merger;
using OpenConnector.Merge.Reader.Transparent;
using NCDC.CommandParserBinder.Command;
using NCDC.CommandParserBinder.MetaData.Schema;
using OpenConnector.ShardingCommon.Core.Rule;
using OpenConnector.ShardingMerge.DAL.Common;
using OpenConnector.ShardingMerge.DAL.Show;
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
