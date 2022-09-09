using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParserBinder.Command;
using OpenConnector.CommandParserBinder.Command.DML;
using OpenConnector.CommandParserBinder.MetaData;
using OpenConnector.CommandParserBinder.MetaData.Schema;
using OpenConnector.Sharding.Rewrites.Abstractions;
using OpenConnector.Sharding.Rewrites.ParameterRewriters.ParameterBuilders;
using OpenConnector.Sharding.Rewrites.Sql.Token.Generator;
using OpenConnector.Sharding.Rewrites.Sql.Token.Generator.Builder;
using OpenConnector.Sharding.Rewrites.Sql.Token.SimpleObject;
using OpenConnector.ShardingAdoNet;

namespace OpenConnector.Sharding.Rewrites;

public sealed class SqlRewriteContext
{
    private readonly ITableMetadataManager _tableMetadataManager;
    private readonly ISqlCommandContext<ISqlCommand> _sqlCommandContext;

    private readonly string _sql;

    private readonly ParameterContext _parameterContext;

    private readonly IParameterBuilder _parameterBuilder;

    private readonly List<SqlToken> _sqlTokens = new List<SqlToken>();

    private readonly SqlTokenGenerators _sqlTokenGenerators = new SqlTokenGenerators();

    public SqlRewriteContext(ITableMetadataManager tableMetadataManager,
        ISqlCommandContext<ISqlCommand> sqlCommandContext, string sql, ParameterContext parameterContext)
    {
        _tableMetadataManager = tableMetadataManager;
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
            _parameterBuilder = new StandardParameterBuilder(parameterContext);
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
        _sqlTokens.AddRange(
            _sqlTokenGenerators.GenerateSqlTokens(_sqlCommandContext, _parameterContext, _tableMetadataManager));
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