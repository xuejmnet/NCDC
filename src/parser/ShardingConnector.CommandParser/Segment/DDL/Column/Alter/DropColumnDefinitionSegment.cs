using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.CommandParser.Segment.DML.Column;

namespace ShardingConnector.CommandParser.Segment.DDL.Column.Alter
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 16:55:39
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class DropColumnDefinitionSegment:IAlterDefinitionSegment
    {
        private readonly int _startIndex;

        private readonly int _stopIndex;
        private ICollection<ColumnSegment> Columns { get; }

        public DropColumnDefinitionSegment(int startIndex, int stopIndex, ICollection<ColumnSegment> columns)
        {
            _startIndex = startIndex;
            _stopIndex = stopIndex;
            Columns = columns;
        }

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
