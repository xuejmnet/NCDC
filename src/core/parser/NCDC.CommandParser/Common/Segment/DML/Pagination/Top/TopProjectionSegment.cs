using NCDC.CommandParser.Common.Segment.DML.Item;
using NCDC.CommandParser.Common.Segment.DML.Pagination.RowNumber;

namespace NCDC.CommandParser.Common.Segment.DML.Pagination.Top
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 12:20:13
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class TopProjectionSegment:IProjectionSegment
    {
        public int StartIndex { get; }
        public int StopIndex { get; }

        private readonly RowNumberValueSegment _top;
    
        private readonly string _alias;

        public TopProjectionSegment(int startIndex, int stopIndex, RowNumberValueSegment top, string alias)
        {
            StartIndex = startIndex;
            StopIndex = stopIndex;
            _top = top;
            _alias = alias;
        }

        public RowNumberValueSegment GetTop()
        {
            return _top;
        }

        public string GetAlias()
        {
            return _alias;
        }
    }
}
