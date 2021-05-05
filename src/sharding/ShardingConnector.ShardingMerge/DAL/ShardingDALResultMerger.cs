using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.Api.Database;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.CommandParser.Command.DAL.Dialect.MySql;
using ShardingConnector.Executor;
using ShardingConnector.Merge.Engine.Merger;
using ShardingConnector.Merge.Reader;
using ShardingConnector.ParserBinder.Command;
using ShardingConnector.ParserBinder.MetaData.Schema;
using ShardingConnector.ShardingCommon.Core.Rule;
using ShardingConnector.ShardingMerge.DAL.Common;

namespace ShardingConnector.ShardingMerge.DAL
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/5/5 10:24:07
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ShardingDALResultMerger:IResultMerger
    {
        private readonly ShardingRule shardingRule;

        public ShardingDALResultMerger(ShardingRule shardingRule)
        {
            this.shardingRule = shardingRule;
        }

        public IMergedEnumerator Merge(List<IQueryEnumerator> queryResults, ISqlCommandContext<ISqlCommand> sqlCommandContext, SchemaMetaData schemaMetaData)
        {
            var dalStatement = sqlCommandContext.GetSqlCommand();
            if (dalStatement is ShowDatabasesCommand showDatabasesCommand) {
                return new SingleLocalDataMergedEnumerator(new List<object>(){ DefaultSchema.LOGIC_NAME });
            }
            if (dalStatement is ShowTablesCommand || dalStatement is ShowTableStatusCommand || dalStatement is ShowIndexCommand) {
                return new LogicTablesMergedResult(shardingRule, sqlStatementContext, schemaMetaData, queryResults);
            }
            if (dalStatement instanceof ShowCreateTableStatement) {
                return new ShowCreateTableMergedResult(shardingRule, sqlStatementContext, schemaMetaData, queryResults);
            }
            return new TransparentMergedResult(queryResults.get(0));
        }
    }
}
