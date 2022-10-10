using NCDC.Base.PriorityQueues;
using NCDC.Basic.TableMetadataManagers;
using NCDC.ShardingMerge.DataReaders.Stream;
using NCDC.ShardingParser.Command.DML;
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

        public OrderByStreamMergedDataReader(List<IStreamDataReader> streamDataReaders, SelectCommandContext selectCommandContext, ITableMetadataManager tableMetadataManager)
        {
            this.OrderByItems = selectCommandContext.GetOrderByContext().GetItems();
            this.OrderByValuesQueue = new Base.PriorityQueues.PriorityQueue<OrderByValue>(streamDataReaders.Count);
            OrderResultSetsToQueue(streamDataReaders, selectCommandContext, tableMetadataManager);
            IsFirstNext = true;
        }

        private void OrderResultSetsToQueue(List<IStreamDataReader> streamDataReaders, SelectCommandContext selectCommandContext, ITableMetadataManager tableMetadataManager)
        {
            foreach (var queryResult in streamDataReaders)
            {
                OrderByValue orderByValue = new OrderByValue(queryResult, OrderByItems, selectCommandContext, tableMetadataManager);
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
            //如果优先级队列里面还有其余的链接直接全部回收掉
            OrderByValuesQueue.Dispose();
        }
    }
}