using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.Base.PriorityQueues;
using ShardingConnector.Merge.Reader.Stream;
using ShardingConnector.CommandParserBinder.Command.DML;
using ShardingConnector.CommandParserBinder.MetaData.Schema;
using ShardingConnector.CommandParserBinder.Segment.Select.OrderBy;
using ShardingConnector.StreamDataReaders;

namespace ShardingConnector.ShardingMerge.DQL.OrderBy
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/5/6 8:20:15
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public class OrderByStreamMergedDataReader : StreamMergedDataReader
    {
        protected ICollection<OrderByItem> OrderByItems { get; }

        protected PriorityQueue<OrderByValue> OrderByValuesQueue { get; }

        protected bool IsFirstNext { get; set; }

        public OrderByStreamMergedDataReader(List<IStreamDataReader> streamDataReaders, SelectCommandContext selectCommandContext, SchemaMetaData schemaMetaData)
        {
            this.OrderByItems = selectCommandContext.GetOrderByContext().GetItems();
            this.OrderByValuesQueue = new PriorityQueue<OrderByValue>(streamDataReaders.Count);
            OrderResultSetsToQueue(streamDataReaders, selectCommandContext, schemaMetaData);
            IsFirstNext = true;
        }

        private void OrderResultSetsToQueue(List<IStreamDataReader> streamDataReaders, SelectCommandContext selectCommandContext, SchemaMetaData schemaMetaData)
        {
            foreach (var queryResult in streamDataReaders)
            {
                OrderByValue orderByValue = new OrderByValue(queryResult, OrderByItems, selectCommandContext, schemaMetaData);
                if (orderByValue.MoveNext())
                {
                    OrderByValuesQueue.Offer(orderByValue);
                }
            }

            SetCurrentStreamDataReader(OrderByValuesQueue.IsEmpty() ? streamDataReaders[0] : OrderByValuesQueue.Peek().GetStreamDataReader());
        }


        public override bool Read()
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

            SetCurrentStreamDataReader(OrderByValuesQueue.Peek().GetStreamDataReader());
            return true;
        }

        public override void Dispose()
        {
            while (!OrderByValuesQueue.IsEmpty())
            {
                var orderByValue = OrderByValuesQueue.Poll();
                orderByValue?.Dispose();
            }
        }
    }
}