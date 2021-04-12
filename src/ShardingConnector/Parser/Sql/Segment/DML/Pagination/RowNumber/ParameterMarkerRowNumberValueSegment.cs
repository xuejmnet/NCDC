using System;
using System.Collections.Generic;
using System.Text;

namespace ShardingConnector.Parser.Sql.Segment.DML.Pagination.RowNumber
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 12:25:22
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ParameterMarkerRowNumberValueSegment:RowNumberValueSegment,IParameterMarkerPaginationValueSegment
    {
        private readonly int _parameterIndex;
        public ParameterMarkerRowNumberValueSegment(int startIndex, int stopIndex,int parameterIndex, bool boundOpened) : base(startIndex, stopIndex, boundOpened)
        {
            this._parameterIndex = parameterIndex;
        }

        public int GetParameterIndex()
        {
            return _parameterIndex;
        }
    }
}
