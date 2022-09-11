using NCDC.Sharding.Routes;

namespace NCDC.Sharding.Rewrites.Sql.Token.Generator.Builder
{
/*
* @Author: xjm
* @Description:
* @Date: Monday, 12 April 2021 21:50:14
* @Email: 326308290@qq.com
*/
    public interface ISqlTokenGeneratorBuilder
    {
        
        ICollection<ISqlTokenGenerator> GetSqlTokenGenerators(RouteContext routeContext);
    }
}