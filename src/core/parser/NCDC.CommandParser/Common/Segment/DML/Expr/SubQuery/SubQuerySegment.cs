using System;
using System.Collections.Generic;
using System.Text;
using NCDC.CommandParser.Common.Command.DML;
using NCDC.CommandParser.Common.Constant;
using NCDC.CommandParser.Common.Segment.DML.Predicate.Value;

namespace NCDC.CommandParser.Common.Segment.DML.Expr.SubQuery
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 17:23:42
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class SubQuerySegment:IExpressionSegment
    {
        public int StartIndex { get; }
        public int StopIndex { get; }
        public SelectCommand Select { get; }
        public SubQueryTypeEnum? SubQueryType { get; set; }

        public SubQuerySegment(int startIndex, int stopIndex, SelectCommand select)
        {
            StartIndex= startIndex;
            StopIndex = stopIndex;
            Select = select;
        }

        public override string ToString()
        {
            return $"{nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}, {nameof(Select)}: {Select}, {nameof(SubQueryType)}: {SubQueryType}";
        }
    }
}
