using System;
using System.Collections.Generic;
using System.Text;

namespace ShardingConnector.Parser.Sql.Segment.DML.Expr.Simple
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 12:50:31
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public class ParameterMarkerExpressionSegment:ISimpleExpressionSegment
    {
        private readonly int _startIndex;

        private readonly int _stopIndex;

        private readonly int _parameterMarkerIndex;

        public ParameterMarkerExpressionSegment(int startIndex, int stopIndex, int parameterMarkerIndex)
        {
            _startIndex = startIndex;
            _stopIndex = stopIndex;
            _parameterMarkerIndex = parameterMarkerIndex;
        }

        public int GetStartIndex()
        {
            return _startIndex;
        }

        public int GetStopIndex()
        {
            return _stopIndex;
        }

        public int GetParameterMarkerIndex()
        {
            return _parameterMarkerIndex;
        }
    }
}
