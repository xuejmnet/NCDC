﻿using NCDC.CommandParser.Common.Segment.DML.Pagination.limit;
using NCDC.ShardingAdoNet;

namespace NCDC.ShardingParser.Segment.Select.Pagination.Engine
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 12:18:14
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class LimitPaginationContextEngine
    {
        /**
         * Create pagination context.
         * 
         * @param limitSegment limit segment
         * @param parameters SQL parameters
         * @return pagination context
         */
        public PaginationContext CreatePaginationContext(LimitSegment limitSegment, ParameterContext parameterContext)
        {
            return new PaginationContext(limitSegment.Offset, limitSegment.RowCount, parameterContext);
        }
    }
}
