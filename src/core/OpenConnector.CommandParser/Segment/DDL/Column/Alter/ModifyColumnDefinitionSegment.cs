using System;
using System.Collections.Generic;
using System.Text;
using OpenConnector.CommandParser.Segment.DDL.Column.Position;

namespace OpenConnector.CommandParser.Segment.DDL.Column.Alter
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 16:57:12
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ModifyColumnDefinitionSegment:IAlterDefinitionSegment
    {
        private readonly int _startIndex;

        private readonly int _stopIndex;

        public ModifyColumnDefinitionSegment(int startIndex, int stopIndex, ColumnDefinitionSegment columnDefinition)
        {
            _startIndex = startIndex;
            _stopIndex = stopIndex;
            ColumnDefinition = columnDefinition;
        }

        public ColumnDefinitionSegment ColumnDefinition { get; }

        public ColumnPositionSegment ColumnPosition { get; set; }

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
