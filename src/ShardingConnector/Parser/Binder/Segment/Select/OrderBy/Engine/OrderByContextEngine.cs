using System;
using System.Collections.Generic;
using System.Linq;
using ShardingConnector.Parser.Binder.Segment.Select.Groupby;
using ShardingConnector.Parser.Sql.Command.DML;
using ShardingConnector.Parser.Sql.Constant;
using ShardingConnector.Parser.Sql.Segment.DML.Item;
using ShardingConnector.Parser.Sql.Segment.DML.Order.Item;

namespace ShardingConnector.Parser.Binder.Segment.Select.OrderBy.Engine
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 21:04:40
* @Email: 326308290@qq.com
*/
    public class OrderByContextEngine
    {
        public OrderByContextEngine()
        {
        }


        public  OrderByContext CreateOrderBy(SelectCommand selectCommand, GroupByContext groupByContext)
        {
            if (null == selectCommand.OrderBy || !selectCommand.OrderBy.GetOrderByItems().Any())
            {
                OrderByContext orderByContext = CreateOrderByContextForDistinctRowWithoutGroupBy(selectCommand, groupByContext);
                return null != orderByContext ? orderByContext : new OrderByContext(groupByContext.GetItems(), groupByContext.GetItems().Any());
            }

            ICollection<OrderByItem> orderByItems = new LinkedList<OrderByItem>();
            foreach (var orderByItemSegment in selectCommand.OrderBy.GetOrderByItems())
            {
                OrderByItem orderByItem = new OrderByItem(orderByItemSegment);
                if (orderByItemSegment is IndexOrderByItemSegment indexOrderByItemSegment)
                {
                    orderByItem.SetIndex(indexOrderByItemSegment.GetColumnIndex());
                }

                orderByItems.Add(orderByItem);
            }

            return new OrderByContext(orderByItems, false);
        }

        private  OrderByContext CreateOrderByContextForDistinctRowWithoutGroupBy(SelectCommand selectCommand, GroupByContext groupByContext)
        {
            if (!groupByContext.GetItems().Any() && selectCommand.Projections.IsDistinctRow())
            {
                int index = 0;
                ICollection<OrderByItem> orderByItems = new LinkedList<OrderByItem>();
                foreach (var projectionSegment in selectCommand.Projections.GetProjections())
                {
                    if (projectionSegment is ColumnProjectionSegment columnProjectionSegment)
                    {
                        var columnOrderByItemSegment = new ColumnOrderByItemSegment(columnProjectionSegment.GetColumn(), OrderDirectionEnum.ASC);
                        OrderByItem item = new OrderByItem(columnOrderByItemSegment);
                        item.SetIndex(index++);
                        orderByItems.Add(item);
                    }
                }

                if (orderByItems.Any())
                {
                    return new OrderByContext(orderByItems, true);
                }
            }

            return null;
        }
    }
}