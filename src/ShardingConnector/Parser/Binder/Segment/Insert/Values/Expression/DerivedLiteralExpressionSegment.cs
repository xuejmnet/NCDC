using ShardingConnector.Parser.Sql.Segment.DML.Expr.Simple;

namespace ShardingConnector.Parser.Binder.Segment.Insert.Values.Expression
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/13 8:57:18
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class DerivedLiteralExpressionSegment: LiteralExpressionSegment, IDerivedSimpleExpressionSegment
    {
        public DerivedLiteralExpressionSegment(object literals) : base(0, 0, literals)
        {
        }
    }
}
