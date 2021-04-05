using System;
using System.Collections.Generic;
using System.Text;

namespace ShardingConnector.Kernels.Route.Rule
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/5 14:31:07
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public interface IBaseRule
    {
        /// <summary>
        /// 获取路由规则
        /// </summary>
        /// <returns></returns>
        IRuleConfiguration GetRuleConfiguration();
    }
}
