using OpenConnector.RewriteEngine.Context;
using OpenConnector.RewriteEngine.Sql.Impl;

namespace OpenConnector.RewriteEngine.Engine
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/13 17:30:17
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class SqlRewriteEngine
    {
        /**
         * Rewrite SQL and parameters.
         *
         * @param sqlRewriteContext SQL rewrite context
         * @return SQL rewrite result
         */
        public SqlRewriteResult Rewrite(SqlRewriteContext sqlRewriteContext)
        {
            return new SqlRewriteResult(new DefaultSqlBuilder(sqlRewriteContext).ToSql(), sqlRewriteContext.GetParameterBuilder().GetParameterContext());
        }
    }
}
