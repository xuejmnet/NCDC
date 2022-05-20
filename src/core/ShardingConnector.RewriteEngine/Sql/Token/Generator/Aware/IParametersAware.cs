using System.Collections.Generic;
using System.Data.Common;

namespace ShardingConnector.RewriteEngine.Sql.Token.Generator.Aware
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 16:14:56
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public interface IParametersAware
    {
        void SetParameters(ParameterContext parameterContext);
    }
}
