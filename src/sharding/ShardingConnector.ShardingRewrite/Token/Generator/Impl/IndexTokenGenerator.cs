using System;
using System.Collections.Generic;
using System.Linq;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.CommandParser.Segment.DDL.Index;
using ShardingConnector.ParserBinder.Command;
using ShardingConnector.ParserBinder.Command.DML;
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
* @Date: Tuesday, 27 April 2021 21:47:55
* @Email: 326308290@qq.com
*/
    public sealed class IndexTokenGenerator : ICollectionSqlTokenGenerator<ISqlCommandContext<ISqlCommand>>, IShardingRuleAware
    {
        private ShardingRule _shardingRule;

        public ICollection<SqlToken> GenerateSqlTokens(ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            ICollection<SqlToken> result = new LinkedList<SqlToken>();
            if (sqlCommandContext is IIndexAvailable indexAvailable)
            {
                foreach (var index in indexAvailable.GetIndexes())
                {
                    result.Add(new IndexToken(index.GetStartIndex(), index.GetStopIndex(), index.Identifier, sqlCommandContext, _shardingRule));
                }
            }

            return result;
        }

        public bool IsGenerateSqlToken(ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            return sqlCommandContext is IIndexAvailable indexAvailable && indexAvailable.GetIndexes().Any();
        }

        public void SetShardingRule(ShardingRule shardingRule)
        {
            this._shardingRule = shardingRule;
        }
    }
}