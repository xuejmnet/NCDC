using System;
using System.Collections.Generic;
using System.Text;
using OpenConnector.CommandParser.Segment.DML.Column;
using OpenConnector.CommandParser.Segment.Generic;
using OpenConnector.CommandParser.Segment.Generic.Table;

namespace OpenConnector.CommandParser.Segment.DDL.Column
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 16:46:14
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ColumnDefinitionSegment: ICreateDefinitionSegment
    {
        private readonly int _startIndex;

        private readonly int _stopIndex;

        public ColumnSegment ColumnName{get;set;}

        public DataTypeSegment DataType { get; set; }

        public  bool PrimaryKey { get; set; }

        public readonly ICollection<SimpleTableSegment> ReferencedTables = new LinkedList<SimpleTableSegment>();

        public ColumnDefinitionSegment( int startIndex,  int stopIndex,  ColumnSegment columnName,  DataTypeSegment dataType,  bool primaryKey)
        {
            this._startIndex = startIndex;
            this._stopIndex = stopIndex;
            this.ColumnName = columnName;
            this.DataType = dataType;
            this.PrimaryKey = primaryKey;
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
