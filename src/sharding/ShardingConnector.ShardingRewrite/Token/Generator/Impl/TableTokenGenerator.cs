using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.ParserBinder.Command;
using ShardingConnector.RewriteEngine.Sql.Token.Generator;
using ShardingConnector.RewriteEngine.Sql.Token.SimpleObject;
using ShardingConnector.ShardingCommon.Core.Rule;
using ShardingConnector.ShardingCommon.Core.Rule.Aware;

namespace ShardingConnector.ShardingRewrite.Token.Generator.Impl
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/27 8:56:49
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class TableTokenGenerator:ICollectionSqlTokenGenerator<ISqlCommandContext<ISqlCommand>>,IShardingRuleAware
    {
        private ShardingRule shardingRule;
        public ICollection<SqlToken> GenerateSqlTokens(ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            return sqlCommandContext is ITableAvailable ? generateSQLTokens((TableAvailable)sqlStatementContext) : Collections.emptyList();
        }

        public bool IsGenerateSqlToken(ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            return true;
        }

        public void SetShardingRule(ShardingRule shardingRule)
        {
            this.shardingRule = shardingRule;
        }



        private ICollection<TableToken> generateSQLTokens(final TableAvailable sqlStatementContext)
        {
            ICollection<TableToken> result = new LinkedList<>();
            for (SimpleTableSegment each : sqlStatementContext.getAllTables())
            {
                if (shardingRule.findTableRule(each.getTableName().getIdentifier().getValue()).isPresent())
                {
                    result.add(new TableToken(each.getStartIndex(), each.getStopIndex(), each.getTableName().getIdentifier(), (SQLStatementContext)sqlStatementContext, shardingRule));
                }
            }
            return result;
        }
    }
}
