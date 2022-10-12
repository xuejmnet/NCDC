using System.Collections.Generic;

namespace NCDC.CommandParser.Segment.DML.Item
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 13:10:51
* @Email: 326308290@qq.com
*/
    public sealed class ProjectionsSegment:ISqlSegment
    {
        public int StartIndex { get; }
        public int StopIndex { get; }
    
        public bool DistinctRow { get; set; }

        public ICollection<IProjectionSegment> Projections = new LinkedList<IProjectionSegment>();

        public ProjectionsSegment(int startIndex, int stopIndex)
        {
            StartIndex = startIndex;
            StopIndex = stopIndex;
        }

        public override string ToString()
        {
            return $"{nameof(Projections)}: {Projections}, {nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}, {nameof(DistinctRow)}: {DistinctRow}";
        }
    }
}