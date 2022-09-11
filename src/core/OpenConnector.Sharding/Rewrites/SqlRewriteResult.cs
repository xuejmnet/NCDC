using OpenConnector.ShardingAdoNet;

namespace OpenConnector.Sharding.Rewrites;


public sealed class SqlRewriteResult
{
    public SqlRewriteResult(string sql, ParameterContext parameterContext)
    {
        Sql = sql;
        ParameterContext = parameterContext;
    }

    public string Sql { get; }
    
    public ParameterContext ParameterContext { get; }
}