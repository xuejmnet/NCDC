using NCDC.Basic.Parsers;
using NCDC.CommandParser.Abstractions;
using NCDC.ShardingParser.Command;
using NCDC.ShardingParser.Command.DML;
using NCDC.Basic.TableMetadataManagers;
using NCDC.ShardingAdoNet;
using NCDC.ShardingRewrite.Abstractions;
using NCDC.ShardingRewrite.ParameterRewriters.ParameterBuilders;
using NCDC.ShardingRewrite.Sql.Token.Generator;
using NCDC.ShardingRewrite.Sql.Token.SimpleObject;
using NCDC.ShardingRoute;

namespace NCDC.ShardingRewrite;

public sealed class SqlRewriteContext
{
    private readonly ITableMetadataManager _tableMetadataManager;
    private readonly RouteContext _routeContext;
    private readonly ISqlCommandContext<ISqlCommand> _sqlCommandContext;

    private readonly string _sql;

    private readonly ParameterContext _parameterContext;

    private readonly IParameterBuilder _parameterBuilder;

    private readonly List<SqlToken> _sqlTokens = new List<SqlToken>();

    private readonly SqlTokenGenerators _sqlTokenGenerators = new SqlTokenGenerators();

    public SqlRewriteContext(ITableMetadataManager tableMetadataManager,RouteContext routeContext)
    {
        _tableMetadataManager = tableMetadataManager;
        _routeContext = routeContext;
        this._sqlCommandContext = routeContext.GetSqlCommandContext();
        this._sql = routeContext.GetSql();
        this._parameterContext = routeContext.GetParameterContext();
        if (_sqlCommandContext is InsertCommandContext insertCommandContext)
        {
            _parameterBuilder = new GroupedParameterBuilder(insertCommandContext.GetGroupedParameters());
        }
        else
        {
            _parameterBuilder = new StandardParameterBuilder(_parameterContext);
        }
    }

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
            _sqlTokenGenerators.GenerateSqlTokens(_sqlCommandContext));
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

    public ParameterContext GetParameterContext()
    {
        return _parameterContext;
    }

    public ISqlCommandContext<ISqlCommand> GetSqlCommandContext()
    {
        return _sqlCommandContext;
    }

    public RouteContext GetRouteContext()
    {
        return _routeContext;
    }
}