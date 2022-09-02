using System;
using System.Collections.Generic;
using System.Text;
using OpenConnector.CommandParser.Segment.DML.Column;
using OpenConnector.CommandParser.Segment.Generic.Table;

namespace OpenConnector.CommandParser.Segment.DDL.Constraint
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 17:07:50
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ConstraintDefinitionSegment:ICreateDefinitionSegment,IAlterDefinitionSegment
    {
        private readonly int _startIndex;

        private readonly int _stopIndex;

        public readonly ICollection<ColumnSegment> PrimaryKeyColumns = new LinkedList<ColumnSegment>();
        public SimpleTableSegment ReferencedTable { get; set; }

        public ConstraintDefinitionSegment(int startIndex, int stopIndex)
        {
            this._startIndex = startIndex;
            this._stopIndex = stopIndex;
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
