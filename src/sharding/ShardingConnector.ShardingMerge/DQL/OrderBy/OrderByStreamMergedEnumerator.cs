using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.Base.PriorityQueues;
using ShardingConnector.Executor;
using ShardingConnector.Merge.Reader.Stream;
using ShardingConnector.ParserBinder.Command.DML;
using ShardingConnector.ParserBinder.MetaData.Schema;
using ShardingConnector.ParserBinder.Segment.Select.OrderBy;

namespace ShardingConnector.ShardingMerge.DQL.OrderBy
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/5/6 8:20:15
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public class OrderByStreamMergedEnumerator : StreamMergedEnumerator
    {
        protected ICollection<OrderByItem> OrderByItems { get; }

        protected PriorityQueue<OrderByValue> OrderByValuesQueue { get; }

        protected bool IsFirstNext { get; set; }

        public OrderByStreamMergedEnumerator(List<IQueryEnumerator> queryResults, SelectCommandContext selectCommandContext, SchemaMetaData schemaMetaData)
        {
            this.OrderByItems = selectCommandContext.GetOrderByContext().GetItems();
            this.OrderByValuesQueue = new PriorityQueue<OrderByValue>(queryResults.Count);
            OrderResultSetsToQueue(queryResults, selectCommandContext, schemaMetaData);
            IsFirstNext = true;
        }

        private void OrderResultSetsToQueue(List<IQueryEnumerator> queryResults, SelectCommandContext selectCommandContext, SchemaMetaData schemaMetaData)
        {
            foreach (var queryResult in queryResults)
            {
                OrderByValue orderByValue = new OrderByValue(queryResult, OrderByItems, selectCommandContext, schemaMetaData);
                if (orderByValue.MoveNext())
                {
                    OrderByValuesQueue.Offer(orderByValue);
                }
            }

            SetCurrentQueryEnumerator(OrderByValuesQueue.IsEmpty() ? queryResults[0] : OrderByValuesQueue.Peek().GetQueryEnumerator());
        }

        public override bool MoveNext()
        {
            if (OrderByValuesQueue.IsEmpty())
            {
                return false;
            }

            if (IsFirstNext)
            {
                IsFirstNext = false;
                return true;
            }

            OrderByValue firstOrderByValue = OrderByValuesQueue.Poll();
            if (firstOrderByValue.MoveNext())
            {
                OrderByValuesQueue.Offer(firstOrderByValue);
            }

            if (OrderByValuesQueue.IsEmpty())
            {
                return false;
            }

            SetCurrentQueryEnumerator(OrderByValuesQueue.Peek().GetQueryEnumerator());
            return true;
        }
    }
}