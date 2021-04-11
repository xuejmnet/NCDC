using System;
using System.Collections.Generic;
using ShardingConnector.Parser.Sql.Segment.DML.Column;
using ShardingConnector.Parser.Sql.Segment.Generic.Table;

namespace ShardingConnector.Parser.Sql.Segment
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 13:23:20
* @Email: 326308290@qq.com
*/
    public sealed class TableFactorSegment:ISqlSegment
    {
        private int startIndex;
    
        private int stopIndex;
    
        private ITableSegment table;
    
        private readonly ICollection<ColumnSegment> columns = new LinkedList<ColumnSegment>();
    
        private ICollection<TableReferenceSegment> tableReferences = new LinkedList<TableReferenceSegment>();

        public int GetStartIndex()
        {
            return startIndex;
        }

        public void SetStartIndex(int startIndex)
        {
            this.startIndex = startIndex;
        }
        public int GetStopIndex()
        {
            return stopIndex;
        }
        public void SetStopIndex(int stopIndex)
        {
            this.stopIndex = stopIndex;
        }

        public ITableSegment GetTable()
        {
            return table;
        }

        public void SetTable(ITableSegment table)
        {
            this.table = table;
        }
    }
}