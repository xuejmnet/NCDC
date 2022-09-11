using NCDC.Basic.Parser.Segment.Select.OrderBy;

namespace NCDC.Basic.Parser.Segment.Select.Groupby
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 15:24:48
* @Email: 326308290@qq.com
*/
    public sealed class GroupByContext
    {
        private readonly ICollection<OrderByItem> _items;
    
        private readonly int _lastIndex;

        public GroupByContext(ICollection<OrderByItem> items, int lastIndex)
        {
            _items = items;
            _lastIndex = lastIndex;
        }

        public ICollection<OrderByItem> GetItems()
        {
            return _items;
        }
        public int GetLastIndex()
        {
            return _lastIndex;
        }
        
    }
}