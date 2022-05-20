﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using ShardingConnector.CommandParser.Segment.DML.Pagination.limit;

namespace ShardingConnector.ParserBinder.Segment.Select.Pagination.Engine
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
        public PaginationContext CreatePaginationContext(LimitSegment limitSegment, IDictionary<string, DbParameter> parameters)
        {
            return new PaginationContext(limitSegment.GetOffset(), limitSegment.GetRowCount(), parameters);
        }
    }
}
