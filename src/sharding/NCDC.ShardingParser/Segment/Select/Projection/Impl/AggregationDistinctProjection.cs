using NCDC.CommandParser.Common.Constant;

namespace NCDC.ShardingParser.Segment.Select.Projection.Impl
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 19:33:53
* @Email: 326308290@qq.com
*/
    public sealed class AggregationDistinctProjection:AggregationProjection
    {
        public  int StartIndex { get; }
    
        public  int StopIndex{ get; }
    
        private readonly string _distinctInnerExpression;
        public AggregationDistinctProjection(int startIndex,int stopIndex,AggregationTypeEnum type, string innerExpression, string alias,string distinctInnerExpression) 
            : base(type, innerExpression, alias)
        {
            StartIndex = startIndex;
            StopIndex = stopIndex;
            _distinctInnerExpression = distinctInnerExpression;
        }
        
        /// <summary>
        /// 获取去重的列标签
        /// </summary>
        /// <returns></returns>
        public string GetDistinctColumnLabel() {
            return GetAlias()??_distinctInnerExpression;
        }

        public string GetDistinctInnerExpression()
        {
            return _distinctInnerExpression;
        }
    }
}