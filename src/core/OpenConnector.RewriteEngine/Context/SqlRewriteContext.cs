using System.Collections.Generic;
using System.Data.Common;
using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParser.Command;
using OpenConnector.CommandParserBinder.Command;
using OpenConnector.CommandParserBinder.Command.DML;
using OpenConnector.CommandParserBinder.MetaData.Schema;
using OpenConnector.RewriteEngine.Parameter.Builder;
using OpenConnector.RewriteEngine.Parameter.Builder.Impl;
using OpenConnector.RewriteEngine.Sql.Token.Generator;
using OpenConnector.RewriteEngine.Sql.Token.Generator.Builder;
using OpenConnector.RewriteEngine.Sql.Token.SimpleObject;
using OpenConnector.ShardingAdoNet;

namespace OpenConnector.RewriteEngine.Context
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
    
        private readonly ParameterContext _parameterContext;

        private readonly IParameterBuilder _parameterBuilder;
    
        private readonly List<SqlToken> _sqlTokens = new List<SqlToken>();

        private readonly SqlTokenGenerators _sqlTokenGenerators = new SqlTokenGenerators();

        public SqlRewriteContext(SchemaMetaData schemaMetaData, ISqlCommandContext<ISqlCommand> sqlCommandContext, string sql, ParameterContext parameterContext)
        {
            this._schemaMetaData = schemaMetaData;
            this._sqlCommandContext = sqlCommandContext;
            this._sql = sql;
            this._parameterContext = parameterContext;
            AddSqlTokenGenerators(new DefaultTokenGeneratorBuilder().GetSqlTokenGenerators());
            if (sqlCommandContext is InsertCommandContext insertCommandContext)
            {
                _parameterBuilder = new GroupedParameterBuilder(insertCommandContext.GetGroupedParameters());
            }
            else
            {
                _parameterBuilder=new StandardParameterBuilder(parameterContext);
            }
        }


        /**
         * Add SQL token generators.
         * 
         * @param sqlTokenGenerators SQL token generators
         */
        public void AddSqlTokenGenerators(ICollection<ISqlTokenGenerator> sqlTokenGenerators)
        {
            this._sqlTokenGenerators.AddAll(sqlTokenGenerators);
        }

        /**
         * Generate SQL tokens.
         */
        public void GenerateSqlTokens()
        {
            _sqlTokens.AddRange(_sqlTokenGenerators.GenerateSqlTokens(_sqlCommandContext, _parameterContext, _schemaMetaData));
        }

        public List<SqlToken> GetSqlTokens()
        {
            return _sqlTokens;
        }

        public string GetSql()
        {
            return _sql;
        }

        public IParameterBuilder GetParameterBuilder()
        {
            return _parameterBuilder;
        }

        public SchemaMetaData GetSchemaMetaData()
        {
            return _schemaMetaData;
        }

        public ParameterContext GetParameterContext()
        {
            return _parameterContext;
        }

        public ISqlCommandContext<ISqlCommand> GetSqlCommandContext()
        {
            return _sqlCommandContext;

        }


    }
}
