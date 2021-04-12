using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.Rewrite.Sql.Token.SimpleObject;

namespace ShardingConnector.Rewrite.Sql.Token.Generator.Aware
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
