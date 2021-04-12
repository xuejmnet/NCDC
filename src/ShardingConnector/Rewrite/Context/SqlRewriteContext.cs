using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.Kernels.MetaData.Schema;
using ShardingConnector.Parser.Binder.Command;
using ShardingConnector.Parser.Sql.Command;
using ShardingConnector.Rewrite.Parameter.Builder.Impl;
using ShardingConnector.Rewrite.Sql.Token.Generator;
using ShardingConnector.Rewrite.Sql.Token.Generator.Builder;
using ShardingConnector.Rewrite.Sql.Token.SimpleObject;

namespace ShardingConnector.Rewrite.Context
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 16:00:25
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class SqlRewriteContext
    {
        private readonly SchemaMetaData _schemaMetaData;
    
        private readonly ISqlCommandContext<ISqlCommand> _sqlCommandContext;
    
        private readonly string _sql;
    
        private readonly IList<object> _parameters;

        private readonly IParameterBuilder _parameterBuilder;
    
        private readonly ICollection<SqlToken> _sqlTokens = new LinkedList<SqlToken>();

        private readonly SqlTokenGenerators _sqlTokenGenerators = new SqlTokenGenerators();

        public SqlRewriteContext(SchemaMetaData schemaMetaData, ISqlCommandContext<ISqlCommand> sqlCommandContext, string sql, IList<object> parameters)
        {
            this._schemaMetaData = schemaMetaData;
            this._sqlCommandContext = sqlCommandContext;
            this._sql = sql;
            this._parameters = parameters;
            AddSQLTokenGenerators(new DefaultTokenGeneratorBuilder().GetSQLTokenGenerators());
            if (sqlCommandContext is InsertStatementContext)
            {
                
            }
            parameterBuilder = sqlStatementContext instanceof InsertStatementContext
                ? new GroupedParameterBuilder(((InsertStatementContext)sqlStatementContext).getGroupedParameters()) : new StandardParameterBuilder(parameters);
        }

        /**
         * Add SQL token generators.
         * 
         * @param sqlTokenGenerators SQL token generators
         */
        public void AddSQLTokenGenerators(ICollection<ISqlTokenGenerator> sqlTokenGenerators)
        {
            this._sqlTokenGenerators.AddAll(sqlTokenGenerators);
        }

        /**
         * Generate SQL tokens.
         */
        public void generateSQLTokens()
        {
            sqlTokens.addAll(sqlTokenGenerators.generateSQLTokens(sqlStatementContext, parameters, schemaMetaData));
        }
    }
}
