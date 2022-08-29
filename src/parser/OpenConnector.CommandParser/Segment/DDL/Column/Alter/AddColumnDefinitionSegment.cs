using System;
using System.Collections.Generic;
using System.Text;
using OpenConnector.CommandParser.Segment.DDL.Column.Position;

namespace OpenConnector.CommandParser.Segment.DDL.Column.Alter
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
        private readonly int _startIndex;

        private readonly int _stopIndex;

        public AddColumnDefinitionSegment(int startIndex, int stopIndex, ICollection<ColumnDefinitionSegment> columnDefinitions)
        {
            this._startIndex = startIndex;
            this._stopIndex = stopIndex;
            ColumnDefinitions = columnDefinitions;
        }

        public  ICollection<ColumnDefinitionSegment> ColumnDefinitions { get; }

        public ColumnPositionSegment ColumnPosition { get; set; }

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
