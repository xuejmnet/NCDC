using ShardingConnector.CommandParser.Segment.DML.Expr.Simple;

namespace ShardingConnector.CommandParserBinder.Segment.Insert.Values.Expression
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
        public DerivedParameterMarkerExpressionSegment(int parameterMarkerIndex,string parameterName) : base(0, 0, parameterMarkerIndex, parameterName)
        {
        }
    }
}
