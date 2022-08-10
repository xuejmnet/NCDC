using System.Text;
using DotNetty.Common.Utilities;

namespace ShardingConnector.ProtocolMysql.Constant;

public class MySqlConstants
{
    public static readonly AttributeKey<MySqlCharacterSet> MYSQL_CHARACTER_SET_ATTRIBUTE_KEY=AttributeKey<MySqlCharacterSet>.ValueOf(typeof(MySqlCharacterSet).FullName);
    public static readonly AttributeKey<int> MYSQL_OPTION_MULTI_STATEMENTS=AttributeKey<int>.ValueOf("MYSQL_OPTION_MULTI_STATEMENTS");
}