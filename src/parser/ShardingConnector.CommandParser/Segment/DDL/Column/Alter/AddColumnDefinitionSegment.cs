using System;
using System.Collections.Generic;
using System.Text;

namespace ShardingConnector.CommandParser.Segment.DDL.Column.Alter
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 16:42:11
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class AddColumnDefinitionSegment: IAlterDefinitionSegment
    {
        private readonly int startIndex;

        private readonly int stopIndex;

        private readonly ICollection<ColumnDefinitionSegment> columnDefinitions;

        private ColumnPositionSegment columnPosition;

        /**
         * Get column position.
         * 
         * @return column position
         */
        public Optional<ColumnPositionSegment> getColumnPosition()
        {
            return Optional.ofNullable(columnPosition);
        }
        public int GetStartIndex()
        {
            throw new NotImplementedException();
        }

        public int GetStopIndex()
        {
            throw new NotImplementedException();
        }
    }
}
