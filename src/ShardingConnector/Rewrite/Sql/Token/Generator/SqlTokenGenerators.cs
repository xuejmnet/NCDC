using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.Kernels.MetaData.Schema;
using ShardingConnector.Parser.Binder.Command;
using ShardingConnector.Parser.Sql.Command;
using ShardingConnector.Rewrite.Sql.Token.Generator.Aware;
using ShardingConnector.Rewrite.Sql.Token.SimpleObject;

namespace ShardingConnector.Rewrite.Sql.Token.Generator
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
    public ICollection<SqlToken> GenerateSQLTokens(ISqlCommandContext<ISqlCommand> sqlCommandContext, IList<object> parameters, SchemaMetaData schemaMetaData)
        {
            ICollection<SqlToken> result = new LinkedList<SqlToken>();

            foreach (var sqlTokenGenerator in _sqlTokenGenerators)
            {
                SetUpSqlTokenGenerator(sqlTokenGenerator, parameters, schemaMetaData, result);
                if (!sqlTokenGenerator.IsGenerateSqlToken(sqlCommandContext))
                {
                    continue;
                }
                if (sqlTokenGenerator is OptionalSQLTokenGenerator) {
                    SQLToken sqlToken = ((OptionalSQLTokenGenerator)each).generateSQLToken(sqlStatementContext);
                    if (!result.contains(sqlToken))
                    {
                        result.add(sqlToken);
                    }
                } else if (each instanceof CollectionSQLTokenGenerator) {
                    result.addAll(((CollectionSQLTokenGenerator)each).generateSQLTokens(sqlStatementContext));
                }
            }
        return result;
    }

    private void SetUpSqlTokenGenerator(ISqlTokenGenerator sqlTokenGenerator, IList<object> parameters, SchemaMetaData schemaMetaData, ICollection<SqlToken> previousSqlTokens)
    {
        if (sqlTokenGenerator is IParametersAware parametersAware) {
            parametersAware.SetParameters(parameters);
        }
        if (sqlTokenGenerator is ISchemaMetaDataAware schemaMetaDataAware) {
            schemaMetaDataAware.SetSchemaMetaData(schemaMetaData);
        }
        if (sqlTokenGenerator is IPreviousSqlTokensAware previousSqlTokensAware) {
            previousSqlTokensAware.SetPreviousSQLTokens(previousSqlTokens);
        }
    }
}
}
