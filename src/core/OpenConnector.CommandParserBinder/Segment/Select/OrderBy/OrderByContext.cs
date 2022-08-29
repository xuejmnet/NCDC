namespace OpenConnector.CommandParserBinder.Segment.Select.OrderBy
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 15:56:34
* @Email: 326308290@qq.com
*/
    public sealed class OrderByContext
    {
        private readonly ICollection<OrderByItem> _items;
    
        private readonly bool _generated;

        public OrderByContext(ICollection<OrderByItem> items, bool generated)
        {
            _items = items;
            _generated = generated;
        }

        public ICollection<OrderByItem> GetItems()
        {
            return _items;
        }

        public bool IsGenerated()
        {
            return _generated;
        }
        
    }
}