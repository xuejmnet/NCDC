using System;
using System.Collections.Generic;
using System.Text;

namespace ShardingConnector.Parser.Sql.Segment.DML.Expr.Complex
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 8:11:29
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public interface IComplexExpressionSegment:IExpressionSegment
    {
        string GetText();
    }
}
