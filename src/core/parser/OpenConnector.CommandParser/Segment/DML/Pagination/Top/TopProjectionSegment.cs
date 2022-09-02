using OpenConnector.CommandParser.Segment.DML.Item;
using OpenConnector.CommandParser.Segment.DML.Pagination.RowNumber;

namespace OpenConnector.CommandParser.Segment.DML.Pagination.Top
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
        private readonly int _startIndex;

        private readonly int _stopIndex;

        private readonly RowNumberValueSegment _top;
    
        private readonly string _alias;

        public TopProjectionSegment(int startIndex, int stopIndex, RowNumberValueSegment top, string @alias)
        {
            _startIndex = startIndex;
            _stopIndex = stopIndex;
            _top = top;
            _alias = alias;
        }

        public int GetStartIndex()
        {
            return _startIndex;
        }

        public int GetStopIndex()
        {
            return _stopIndex;
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
