using NCDC.Base.PriorityQueues;
using NCDC.ShardingMerge.DataReaders.Stream;
using NCDC.ShardingParser.Command.DML;
using NCDC.ShardingParser.MetaData.Schema;
using NCDC.ShardingParser.Segment.Select.OrderBy;
using NCDC.StreamDataReaders;

namespace NCDC.ShardingMerge.DataReaderMergers.DQL.OrderBy
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

        protected Base.PriorityQueues.PriorityQueue<OrderByValue> OrderByValuesQueue { get; }

        protected bool IsFirstNext { get; set; }

        public OrderByStreamMergedDataReader(List<IStreamDataReader> streamDataReaders, SelectCommandContext selectCommandContext, SchemaMetaData schemaMetaData)
        {
            this.OrderByItems = selectCommandContext.GetOrderByContext().GetItems();
            this.OrderByValuesQueue = new Base.PriorityQueues.PriorityQueue<OrderByValue>(streamDataReaders.Count);
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