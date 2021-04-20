using System.Collections.Generic;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.Extensions;
using ShardingConnector.ParserBinder.Command;
using ShardingConnector.ParserBinder.MetaData.Schema;
using ShardingConnector.RewriteEngine.Sql.Token.Generator.Aware;
using ShardingConnector.RewriteEngine.Sql.Token.SimpleObject;

namespace ShardingConnector.RewriteEngine.Sql.Token.Generator
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 16:08:16
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class SqlTokenGenerators
    {
        private readonly ICollection<ISqlTokenGenerator> _sqlTokenGenerators = new LinkedList<ISqlTokenGenerator>();

        /**
         * Add all SQL token generators.
         * 
         * @param sqlTokenGenerators SQL token generators
         */
        public void AddAll(ICollection<ISqlTokenGenerator> sqlTokenGenerators)
        {
            foreach (var item in sqlTokenGenerators)
            {
                if (!ContainsClass(item))
                {
                    this._sqlTokenGenerators.Add(item);
                }
            }
        }

        private bool ContainsClass(ISqlTokenGenerator sqlTokenGenerator)
        {
            foreach (var item in _sqlTokenGenerators)
            {
                if (item.GetType() == sqlTokenGenerator.GetType())
                    return true;
            }

            return false;
        }

        /**
         * Generate SQL tokens.
         *
         * @param sqlStatementContext SQL statement context
         * @param parameters SQL parameters
         * @param schemaMetaData schema meta data
         * @return SQL tokens
         */
        public ICollection<SqlToken> GenerateSqlTokens(ISqlCommandContext<ISqlCommand> sqlCommandContext, IList<object> parameters, SchemaMetaData schemaMetaData)
        {
            ICollection<SqlToken> result = new LinkedList<SqlToken>();

            foreach (var sqlTokenGenerator in _sqlTokenGenerators)
            {
                SetUpSqlTokenGenerator(sqlTokenGenerator, parameters, schemaMetaData, result);
                if (!sqlTokenGenerator.IsGenerateSqlToken(sqlCommandContext))
                {
                    continue;
                }

                if (sqlTokenGenerator is IOptionalSQLTokenGenerator<ISqlCommandContext<ISqlCommand>> optionalSqlTokenGenerator)
                {
                    SqlToken sqlToken = optionalSqlTokenGenerator.GenerateSQLToken(sqlCommandContext);
                    if (!result.Contains(sqlToken))
                    {
                        result.Add(sqlToken);
                    }
                }
                else if (sqlTokenGenerator is ICollectionSqlTokenGenerator<ISqlCommandContext<ISqlCommand>> collectionSqlTokenGenerator)
                {
                    result.AddAll(collectionSqlTokenGenerator.GenerateSQLTokens(sqlCommandContext));
                }
            }

            return result;
        }

        private void SetUpSqlTokenGenerator(ISqlTokenGenerator sqlTokenGenerator, IList<object> parameters, SchemaMetaData schemaMetaData, ICollection<SqlToken> previousSqlTokens)
        {
            if (sqlTokenGenerator is IParametersAware parametersAware)
            {
                parametersAware.SetParameters(parameters);
            }

            if (sqlTokenGenerator is ISchemaMetaDataAware schemaMetaDataAware)
            {
                schemaMetaDataAware.SetSchemaMetaData(schemaMetaData);
            }

            if (sqlTokenGenerator is IPreviousSqlTokensAware previousSqlTokensAware)
            {
                previousSqlTokensAware.SetPreviousSQLTokens(previousSqlTokens);
            }
        }
    }
}