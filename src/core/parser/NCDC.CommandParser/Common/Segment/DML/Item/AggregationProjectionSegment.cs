using NCDC.CommandParser.Common.Constant;
using NCDC.CommandParser.Common.Segment.DML.Expr;
using NCDC.CommandParser.Common.Segment.Generic;

namespace NCDC.CommandParser.Common.Segment.DML.Item
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 8:41:16
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public class AggregationProjectionSegment:IProjectionSegment,IAliasAvailable,IExpressionSegment
    {
        public int StartIndex { get; }
        public int StopIndex { get; }
        public AggregationTypeEnum Type { get; }
        public string InnerExpression { get; }
        public ICollection<IExpressionSegment> Parameters = new LinkedList<IExpressionSegment>();


        private AliasSegment? _alias;

        public AggregationProjectionSegment(int startIndex, int stopIndex, AggregationTypeEnum type, string innerExpression)
        {
            StartIndex = startIndex;
            StopIndex = stopIndex;
            Type = type;
            InnerExpression = innerExpression;
        }
        public string? GetAlias()
        {
            return _alias?.IdentifierValue.Value;
        }

        public void SetAlias(AliasSegment? alias)
        {
            _alias = alias;
        }

    }
}
