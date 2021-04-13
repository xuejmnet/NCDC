using ShardingConnector.Parser.Sql.Segment.DML.Expr.Simple;

namespace ShardingConnector.Parser.Binder.Segment.Insert.Values.Expression
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/13 8:58:29
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class DerivedParameterMarkerExpressionSegment: ParameterMarkerExpressionSegment,IDerivedSimpleExpressionSegment
    {
        public DerivedParameterMarkerExpressionSegment(int parameterMarkerIndex) : base(0, 0, parameterMarkerIndex)
        {
        }
    }
}
