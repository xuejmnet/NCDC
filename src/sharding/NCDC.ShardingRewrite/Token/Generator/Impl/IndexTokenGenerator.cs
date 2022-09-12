using NCDC.CommandParser.Abstractions;
using NCDC.ShardingParser.Command;
using NCDC.Basic.TableMetadataManagers;
using NCDC.ShardingRewrite.Sql.Token.Generator;
using NCDC.ShardingRewrite.Sql.Token.SimpleObject;
using NCDC.ShardingRewrite.Token.SimpleObject;

namespace NCDC.ShardingRewrite.Token.Generator.Impl
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 27 April 2021 21:47:55
* @Email: 326308290@qq.com
*/
    public sealed class IndexTokenGenerator : ICollectionSqlTokenGenerator<ISqlCommandContext<ISqlCommand>>
    {
        private readonly ITableMetadataManager _tableMetadataManager;

        public IndexTokenGenerator(ITableMetadataManager tableMetadataManager)
        {
            _tableMetadataManager = tableMetadataManager;
        }

        public ICollection<SqlToken> GenerateSqlTokens(ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            ICollection<SqlToken> result = new LinkedList<SqlToken>();
            if (sqlCommandContext is IIndexAvailable indexAvailable)
            {
                foreach (var index in indexAvailable.GetIndexes())
                {
                    result.Add(new IndexToken(index.GetStartIndex(), index.GetStopIndex(), index.Identifier, sqlCommandContext, _tableMetadataManager));
                }
            }

            return result;
        }

        public bool IsGenerateSqlToken(ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            return sqlCommandContext is IIndexAvailable indexAvailable && indexAvailable.GetIndexes().Any();
        }
    }
}