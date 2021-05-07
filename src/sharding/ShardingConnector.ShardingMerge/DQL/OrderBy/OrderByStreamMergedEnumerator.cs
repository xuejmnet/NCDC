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
        protected ICollection<OrderByItem> orderByItems { get; }

        protected PriorityQueue<OrderByValue> orderByValuesQueue { get; }

        protected bool isFirstNext { get; set; }

        public OrderByStreamMergedEnumerator(List<IQueryEnumerator> queryResults, SelectCommandContext selectCommandContext, SchemaMetaData schemaMetaData)
        {
            this.orderByItems = selectCommandContext.GetOrderByContext().GetItems();
            this.orderByValuesQueue = new PriorityQueue<OrderByValue>(queryResults.Count);
            OrderResultSetsToQueue(queryResults, selectCommandContext, schemaMetaData);
            isFirstNext = true;
        }

        private void OrderResultSetsToQueue(List<IQueryEnumerator> queryResults, SelectCommandContext selectCommandContext, SchemaMetaData schemaMetaData)
        {
            foreach (var queryResult in queryResults)
            {
                OrderByValue orderByValue = new OrderByValue(queryResult, orderByItems, selectCommandContext, schemaMetaData);
                if (orderByValue.MoveNext())
                {
                    orderByValuesQueue.Offer(orderByValue);
                }
            }

            SetCurrentQueryEnumerator(orderByValuesQueue.IsEmpty() ? queryResults[0] : orderByValuesQueue.Peek().GetQueryEnumerator());
        }

        public override bool MoveNext()
        {
            if (orderByValuesQueue.IsEmpty())
            {
                return false;
            }

            if (isFirstNext)
            {
                isFirstNext = false;
                return true;
            }

            OrderByValue firstOrderByValue = orderByValuesQueue.Poll();
            if (firstOrderByValue.MoveNext())
            {
                orderByValuesQueue.Offer(firstOrderByValue);
            }

            if (orderByValuesQueue.IsEmpty())
            {
                return false;
            }

            SetCurrentQueryEnumerator(orderByValuesQueue.Peek().GetQueryEnumerator());
            return true;
        }
    }
}