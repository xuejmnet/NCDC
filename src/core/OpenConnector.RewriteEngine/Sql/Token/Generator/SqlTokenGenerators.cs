using System.Collections.Generic;
using System.Data.Common;
using OpenConnector.CommandParser.Command;
using OpenConnector.CommandParserBinder.Command;
using OpenConnector.CommandParserBinder.MetaData.Schema;
using OpenConnector.Extensions;
using OpenConnector.RewriteEngine.Sql.Token.Generator.Aware;
using OpenConnector.RewriteEngine.Sql.Token.SimpleObject;
using OpenConnector.ShardingAdoNet;

namespace OpenConnector.RewriteEngine.Sql.Token.Generator
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
        public ICollection<SqlToken> GenerateSqlTokens(ISqlCommandContext<ISqlCommand> sqlCommandContext, ParameterContext parameterContext, SchemaMetaData schemaMetaData)
        {
            ICollection<SqlToken> result = new LinkedList<SqlToken>();

            foreach (var sqlTokenGenerator in _sqlTokenGenerators)
            {
                SetUpSqlTokenGenerator(sqlTokenGenerator, parameterContext, schemaMetaData, result);
                if (!sqlTokenGenerator.IsGenerateSqlToken(sqlCommandContext))
                {
                    continue;
                }

                if (sqlTokenGenerator is IOptionalSqlTokenGenerator optionalSqlTokenGenerator)
                {
                    SqlToken sqlToken = optionalSqlTokenGenerator.GenerateSqlToken(sqlCommandContext);
                    if (!result.Contains(sqlToken))
                    {
                        result.Add(sqlToken);
                    }
                }
                else if (sqlTokenGenerator is ICollectionSqlTokenGenerator collectionSqlTokenGenerator)
                {
                    result.AddAll(collectionSqlTokenGenerator.GenerateSqlTokens(sqlCommandContext));
                }
            }

            return result;
        }

        private void SetUpSqlTokenGenerator(ISqlTokenGenerator sqlTokenGenerator, ParameterContext parameterContext, SchemaMetaData schemaMetaData, ICollection<SqlToken> previousSqlTokens)
        {
            if (sqlTokenGenerator is IParametersAware parametersAware)
            {
                parametersAware.SetParameterContext(parameterContext);
            }

            if (sqlTokenGenerator is ISchemaMetaDataAware schemaMetaDataAware)
            {
                schemaMetaDataAware.SetSchemaMetaData(schemaMetaData);
            }

            if (sqlTokenGenerator is IPreviousSqlTokensAware previousSqlTokensAware)
            {
                previousSqlTokensAware.SetPreviousSqlTokens(previousSqlTokens);
            }
        }
    }
}