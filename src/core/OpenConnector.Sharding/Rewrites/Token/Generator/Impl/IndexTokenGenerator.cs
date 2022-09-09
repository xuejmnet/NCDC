using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParserBinder.Command;
using OpenConnector.CommandParserBinder.MetaData;
using OpenConnector.Sharding.Rewrites.Sql.Token.Generator;
using OpenConnector.Sharding.Rewrites.Sql.Token.SimpleObject;
using OpenConnector.Sharding.Rewrites.Token.SimpleObject;

namespace OpenConnector.Sharding.Rewrites.Token.Generator.Impl
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