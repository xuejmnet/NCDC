using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.Api.Database;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.CommandParser.Command.DAL.Dialect.MySql;
using ShardingConnector.Executor;
using ShardingConnector.Merge.Engine.Merger;
using ShardingConnector.Merge.Reader;
using ShardingConnector.Merge.Reader.Transparent;
using ShardingConnector.ParserBinder.Command;
using ShardingConnector.ParserBinder.MetaData.Schema;
using ShardingConnector.ShardingCommon.Core.Rule;
using ShardingConnector.ShardingMerge.DAL.Common;
using ShardingConnector.ShardingMerge.DAL.Show;

namespace ShardingConnector.ShardingMerge.DAL
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/5/5 10:24:07
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ShardingDALEnumeratorMerger:IResultMerger
    {
        private readonly ShardingRule shardingRule;

        public ShardingDALEnumeratorMerger(ShardingRule shardingRule)
        {
            this.shardingRule = shardingRule;
        }

        public IMergedEnumerator Merge(List<IQueryEnumerator> queryEnumerators, ISqlCommandContext<ISqlCommand> sqlCommandContext, SchemaMetaData schemaMetaData)
        {
            var dalStatement = sqlCommandContext.GetSqlCommand();
            if (dalStatement is ShowDatabasesCommand showDatabasesCommand) {
                return new SingleLocalDataMergedEnumerator(new List<object>(){ DefaultSchema.LOGIC_NAME });
            }
            if (dalStatement is ShowTablesCommand || dalStatement is ShowTableStatusCommand || dalStatement is ShowIndexCommand) {
                return new LogicTablesMergedEnumerator(shardingRule, schemaMetaData,sqlCommandContext, queryEnumerators);
            }
            if (dalStatement is ShowCreateTableCommand) {
                return new ShowCreateTableMergedEnumerator(shardingRule,schemaMetaData, sqlCommandContext, queryEnumerators);
            }
            return new TransparentMergedEnumerator(queryEnumerators[0]);
        }
    }
}
