using System;
using System.Collections.Generic;
using System.Text;

namespace ShardingConnector.Rewrite.Sql.Token.Generator.Aware
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
        void SetParameters(ICollection<object> parameters);
    }
}
