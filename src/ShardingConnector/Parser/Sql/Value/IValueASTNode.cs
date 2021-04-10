using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.Kernels.Parse.SqlExpression;

namespace ShardingConnector.Parser.Sql.Value
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/10 10:09:53
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public interface IValueASTNode<T>:IASTNode
    {
        T GetValue();
    }
}
