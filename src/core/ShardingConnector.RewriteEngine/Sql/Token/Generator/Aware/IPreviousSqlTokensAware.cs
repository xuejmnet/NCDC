using System.Collections.Generic;
using ShardingConnector.RewriteEngine.Sql.Token.SimpleObject;

namespace ShardingConnector.RewriteEngine.Sql.Token.Generator.Aware
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 16:15:43
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
   public interface IPreviousSqlTokensAware
    {
        void SetPreviousSQLTokens(ICollection<SqlToken> previousSQLTokens);
    }
}
