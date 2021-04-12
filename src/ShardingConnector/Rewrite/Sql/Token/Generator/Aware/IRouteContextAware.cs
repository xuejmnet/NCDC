﻿using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.Kernels.Route;

namespace ShardingConnector.Rewrite.Sql.Token.Generator.Aware
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 16:16:17
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public interface IRouteContextAware
    {
        void SetRouteContext(RouteContext routeContext);
    }
}
