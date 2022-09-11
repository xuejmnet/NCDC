using System;
using System.Collections.Generic;
using System.Text;
using NCDC.CommandParser.Command.DML;
using NCDC.CommandParser.Segment.DML.Predicate.Value;

namespace NCDC.CommandParser.Segment.DML.Expr.SubQuery
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 17:23:42
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class SubQuerySegment:ISqlSegment,IPredicateRightValue
    {
        private readonly int _startIndex;

        private readonly int _stopIndex;

        public SubQuerySegment(int startIndex, int stopIndex, SelectCommand @select)
        {
            this._startIndex = startIndex;
            this._stopIndex = stopIndex;
            Select = @select;
        }

        public SelectCommand Select { get; }
        public int GetStartIndex()
        {
            return _startIndex;
        }

        public int GetStopIndex()
        {
            return _stopIndex;
        }
    }
}
