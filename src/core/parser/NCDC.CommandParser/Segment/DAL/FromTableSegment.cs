using NCDC.CommandParser.Segment.Generic.Table;

namespace NCDC.CommandParser.Segment.DAL
{
/*
* @Author: xjm
* @Description:
* @Date: Monday, 12 April 2021 22:28:55
* @Email: 326308290@qq.com
*/
    public sealed class FromTableSegment:ISqlSegment
    {
        private int startIndex;
    
        private int stopIndex;
    
        private SimpleTableSegment table;
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

        public SimpleTableSegment GetTable()
        {
            return table;
        }

        public void SetTable(SimpleTableSegment table)
        {
            this.table = table;
        }
    }
}