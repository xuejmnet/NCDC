using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.Rewrite.Context;
using ShardingConnector.Rewrite.Sql.Impl;

namespace ShardingConnector.Rewrite.Engine
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
            return new SqlRewriteResult(new DefaultSqlBuilder(sqlRewriteContext).ToSql(), sqlRewriteContext.GetParameterBuilder().GetParameters());
        }
    }
}
