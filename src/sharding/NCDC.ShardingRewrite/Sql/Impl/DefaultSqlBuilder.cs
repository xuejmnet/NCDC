
using NCDC.ShardingRewrite.Sql.Token.SimpleObject;

namespace NCDC.ShardingRewrite.Sql.Impl
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 13 April 2021 20:50:37
* @Email: 326308290@qq.com
*/
    public sealed class DefaultSqlBuilder:AbstractSqlBuilder
    {
        public DefaultSqlBuilder(SqlRewriteContext context) : base(context)
        {
        }

        protected override string GetSqlTokenText(SqlToken sqlToken)
        {
            return sqlToken.ToString();
        }
    }
}