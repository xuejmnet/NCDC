using NCDC.CommandParser.Command.DML;
using NCDC.CommandParser.Segment.DML.Order.Item;
using NCDC.ShardingParser.Segment.Select.OrderBy;

namespace NCDC.ShardingParser.Segment.Select.Groupby.Engine
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 21:03:35
* @Email: 326308290@qq.com
*/
    public sealed class GroupByContextEngine
    {
        public GroupByContextEngine()
        {
            
        }
        
        
        public  GroupByContext CreateGroupByContext(SelectCommand selectCommand) {
            if (selectCommand.GroupBy==null) {
                return new GroupByContext(new LinkedList<OrderByItem>(), 0);
            }
            ICollection<OrderByItem> groupByItems = new LinkedList<OrderByItem>();
            foreach (var item in selectCommand.GroupBy.GetGroupByItems())
            {
                var orderByItem = new OrderByItem(item);
                if (item is IndexOrderByItemSegment indexOrderByItemSegment)
                {
                    orderByItem.SetIndex(indexOrderByItemSegment.GetColumnIndex());
                }
                groupByItems.Add(orderByItem);
            }
            return new GroupByContext(groupByItems, selectCommand.GroupBy.GetStopIndex());
        }
    }
}