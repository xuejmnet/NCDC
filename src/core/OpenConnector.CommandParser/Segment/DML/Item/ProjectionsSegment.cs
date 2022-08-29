using System.Collections.Generic;

namespace OpenConnector.CommandParser.Segment.DML.Item
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 13:10:51
* @Email: 326308290@qq.com
*/
    public sealed class ProjectionsSegment:ISqlSegment
    {
        
        private readonly int _startIndex;
    
        private readonly int _stopIndex;
    
        private bool distinctRow;

        private readonly ICollection<IProjectionSegment> _projections = new LinkedList<IProjectionSegment>();

        public ProjectionsSegment(int startIndex, int stopIndex)
        {
            _startIndex = startIndex;
            _stopIndex = stopIndex;
        }

        public int GetStartIndex()
        {
            return _startIndex;
        }

        public int GetStopIndex()
        {
            return _stopIndex;
        }

        public void SetDistinctRow(bool isDistinctRow)
        {
            distinctRow = isDistinctRow;
        }

        public ICollection<IProjectionSegment> GetProjections()
        {
            return _projections;
        }

        public bool IsDistinctRow()
        {
            return distinctRow;
        }
    }
}