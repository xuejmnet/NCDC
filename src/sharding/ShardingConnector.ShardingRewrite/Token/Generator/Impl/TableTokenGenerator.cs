using System;
using System.Collections.Generic;
using System.Text;

using ShardingConnector.CommandParser.Command;
using ShardingConnector.CommandParser.Segment.Generic.Table;
using ShardingConnector.CommandParserBinder.Command;
using ShardingConnector.RewriteEngine.Sql.Token.Generator;
using ShardingConnector.RewriteEngine.Sql.Token.SimpleObject;
using ShardingConnector.ShardingCommon.Core.Rule;
using ShardingConnector.ShardingCommon.Core.Rule.Aware;
using ShardingConnector.ShardingRewrite.Token.SimpleObject;

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
            if (sqlCommandContext is ITableAvailable tableAvailable)
            {
                return GenerateSqlTokens(tableAvailable);
            }

            return new List<SqlToken>(0);
        }

        public bool IsGenerateSqlToken(ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            return true;
        }

        public void SetShardingRule(ShardingRule shardingRule)
        {
            this.shardingRule = shardingRule;
        }



        private ICollection<SqlToken> GenerateSqlTokens(ITableAvailable sqlStatementContext)
        {
            ICollection<SqlToken> result = new LinkedList<SqlToken>();
            foreach (var simpleTableSegment in sqlStatementContext.GetAllTables())
            {
                if (shardingRule.FindTableRule(simpleTableSegment.GetTableName().GetIdentifier().GetValue())!=null)
                {
                    result.Add(new TableToken(simpleTableSegment.GetStartIndex(), simpleTableSegment.GetStopIndex(), simpleTableSegment.GetTableName().GetIdentifier(), (ISqlCommandContext<ISqlCommand>)sqlStatementContext, shardingRule));
                }
            }
            return result;
        }
    }
}
