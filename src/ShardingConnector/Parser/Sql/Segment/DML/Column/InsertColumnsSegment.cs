﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ShardingConnector.Parser.Sql.Segment.DML.Column
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/13 7:38:42
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class InsertColumnsSegment:ISqlSegment
    {
        private readonly int _startIndex;

        private readonly int _stopIndex;

        private readonly List<ColumnSegment> _columns;

        public InsertColumnsSegment(int startIndex, int stopIndex, List<ColumnSegment> columns)
        {
            _startIndex = startIndex;
            _stopIndex = stopIndex;
            _columns = columns;
        }

        public int GetStartIndex()
        {
            return _startIndex;
        }

        public int GetStopIndex()
        {
            return _stopIndex;
        }

        public List<ColumnSegment> GetColumns()
        {
            return _columns;
        }
    }
}