using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.Kernels.MetaData.Schema;
using ShardingConnector.Parser.Binder.Command;
using ShardingConnector.Parser.Sql.Command;
using ShardingConnector.Rewrite.Parameter.Builder.Impl;
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

        private readonly SQLTokenGenerators sqlTokenGenerators = new SQLTokenGenerators();

        public SQLRewriteContext(final SchemaMetaData schemaMetaData, final SQLStatementContext sqlStatementContext, final String sql, final List<Object> parameters)
        {
            this.schemaMetaData = schemaMetaData;
            this.sqlStatementContext = sqlStatementContext;
            this.sql = sql;
            this.parameters = parameters;
            addSQLTokenGenerators(new DefaultTokenGeneratorBuilder().getSQLTokenGenerators());
            parameterBuilder = sqlStatementContext instanceof InsertStatementContext
                ? new GroupedParameterBuilder(((InsertStatementContext)sqlStatementContext).getGroupedParameters()) : new StandardParameterBuilder(parameters);
        }

        /**
         * Add SQL token generators.
         * 
         * @param sqlTokenGenerators SQL token generators
         */
        public void addSQLTokenGenerators(final Collection<SQLTokenGenerator> sqlTokenGenerators)
        {
            this.sqlTokenGenerators.addAll(sqlTokenGenerators);
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
