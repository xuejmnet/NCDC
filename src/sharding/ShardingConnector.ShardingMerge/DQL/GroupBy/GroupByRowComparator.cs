using System;
using System.Collections.Generic;
using ShardingConnector.Base;
using ShardingConnector.Extensions;
using ShardingConnector.Merge.Reader.Memory;
using ShardingConnector.ParserBinder.Command.DML;
using ShardingConnector.ParserBinder.Segment.Select.OrderBy;
using ShardingConnector.ShardingMerge.DQL.OrderBy;

namespace ShardingConnector.ShardingMerge.DQL.GroupBy
{
/*
* @Author: xjm
* @Description:
* @Date: Friday, 07 May 2021 22:32:05
* @Email: 326308290@qq.com
*/
    public sealed class GroupByRowComparator : IComparer<MemoryQueryResultRow>
    {
        private readonly SelectCommandContext selectCommandContext;

        private readonly List<bool> valueCaseSensitive;

        public GroupByRowComparator(SelectCommandContext selectCommandContext, List<bool> valueCaseSensitive)
        {
            this.selectCommandContext = selectCommandContext;
            this.valueCaseSensitive = valueCaseSensitive;
        }

        public int Compare(MemoryQueryResultRow x, MemoryQueryResultRow y)
        {
            if (!selectCommandContext.GetOrderByContext().GetItems().IsEmpty())
            {
                return Compare(x, y, selectCommandContext.GetOrderByContext().GetItems());
            }

            return Compare(x, y, selectCommandContext.GetGroupByContext().GetItems());
        }

        private int Compare(MemoryQueryResultRow o1, MemoryQueryResultRow o2, ICollection<OrderByItem> orderByItems)
        {
            foreach (var orderByItem in orderByItems)
            {
                Object orderValue1 = o1.GetCell(orderByItem.GetIndex());
                ShardingAssert.Else(null == orderValue1 || orderValue1 is IComparable, "Order by value must implements Comparable");
                Object orderValue2 = o2.GetCell(orderByItem.GetIndex());
                ShardingAssert.Else(null == orderValue2 || orderValue2 is IComparable, "Order by value must implements Comparable");
                int result = CompareUtil.CompareTo((IComparable) orderValue1, (IComparable) orderValue2, orderByItem.GetSegment().GetOrderDirection(),
                    orderByItem.GetSegment().GetNullOrderDirection(), valueCaseSensitive[orderByItem.GetIndex()]);
                if (0 != result)
                {
                    return result;
                }
            }

            return 0;
        }
    }
}