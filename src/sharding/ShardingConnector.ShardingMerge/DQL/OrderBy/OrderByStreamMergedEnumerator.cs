using System;
using System.Collections.Generic;
using System.Text;
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
    public class OrderByStreamMergedEnumerator:StreamMergedEnumerator
    {
        protected readonly ICollection<OrderByItem> orderByItems { get;}

        protected readonly Queue<OrderByValue> orderByValuesQueue { get; }

        protected bool isFirstNext { get; set; }

        public OrderByStreamMergedResult(List<IQueryEnumerator> queryResults,  SelectCommandContext selectCommandContext,  SchemaMetaData schemaMetaData)
        {
            this.orderByItems = selectCommandContext.GetOrderByContext().GetItems();
            this.orderByValuesQueue = new PriorityQueue<>(queryResults.size());
        orderResultSetsToQueue(queryResults, selectStatementContext, schemaMetaData);
        isFirstNext = true;
    }
    public override bool MoveNext()
        {
            throw new NotImplementedException();
        }
    }
}
