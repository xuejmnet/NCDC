using ShardingConnector.Protocol.Core.Error;

namespace ShardingConnector.Protocol.MySql.Constant;

public class MySqlServerErrorCodeProvider
{
    private static readonly MySqlServerErrorCodeProvider _instance;

    static MySqlServerErrorCodeProvider()
    {
        _instance=  new MySqlServerErrorCodeProvider();
    }
    public static MySqlServerErrorCodeProvider GetInstance() => _instance;
    
    
    public ISqlErrorCode ER_DBACCESS_DENIED_ERROR { get; }

    public MySqlServerErrorCodeProvider()
    {
        ER_DBACCESS_DENIED_ERROR = new SqlErrorCode(1044, "42000", "Access denied for user '%s'@'%s' to database '%s'");
    }
}