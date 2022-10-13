using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Common.Command;
using NCDC.ShardingParser.Command;

namespace NCDC.ShardingRewrite.Sql.Token.Generator
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 16:08:57
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public interface ISqlTokenGenerator
    {
        bool IsGenerateSqlToken(ISqlCommandContext<ISqlCommand> sqlCommandContext);
    }
}
