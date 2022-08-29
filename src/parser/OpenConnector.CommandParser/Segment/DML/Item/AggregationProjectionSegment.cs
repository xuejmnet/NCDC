using OpenConnector.CommandParser.Constant;
using OpenConnector.CommandParser.Segment.Generic;

namespace OpenConnector.CommandParser.Segment.DML.Item
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 8:41:16
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public class AggregationProjectionSegment:IProjectionSegment,IAliasAvailable
    {
        private readonly int _startIndex;

        private readonly int _stopIndex;

        private readonly AggregationTypeEnum _type;
    
        private readonly int _innerExpressionStartIndex;

        private AliasSegment alias;

        public AggregationProjectionSegment(int startIndex, int stopIndex, AggregationTypeEnum type, int innerExpressionStartIndex)
        {
            _startIndex = startIndex;
            _stopIndex = stopIndex;
            _type = type;
            _innerExpressionStartIndex = innerExpressionStartIndex;
        }

        public int GetStartIndex()
        {
            return _startIndex;
        }

        public int GetStopIndex()
        {
            return _stopIndex;
        }

        public string GetAlias()
        {
            return alias?.GetIdentifier().GetValue();
        }

        public void SetAlias(AliasSegment alias)
        {
            this.alias = alias;
        }

        public int GetInnerExpressionStartIndex()
        {
            return _innerExpressionStartIndex;
        }

        public AggregationTypeEnum GetAggregationType()
        {
            return _type;
        }
    }
}
