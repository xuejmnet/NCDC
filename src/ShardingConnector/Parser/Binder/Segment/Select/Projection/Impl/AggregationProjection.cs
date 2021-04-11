using System;
using System.Collections.Generic;
using ShardingConnector.Parser.Sql.Constant;

namespace ShardingConnector.Parser.Binder.Segment.Select.Projection.Impl
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 16:19:00
* @Email: 326308290@qq.com
*/
    public class AggregationProjection:IProjection
    {
        private readonly AggregationTypeEnum _type;
    
        private readonly string _innerExpression;
    
        private readonly string _alias;
    
        private readonly List<AggregationProjection> _derivedAggregationProjections = new List<AggregationProjection>(2);
    
        private int index = -1;
        public string GetExpression()
        {
            throw new NotImplementedException();
        }

        public string GetAlias()
        {
            throw new NotImplementedException();
        }

        public string GetColumnLabel()
        {
            throw new NotImplementedException();
        }

        public void SetIndex(int index)
        {
            this.index = index;
        }
    }
}