using System.Collections.Generic;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.Executor;
using ShardingConnector.Merge.Reader.Memory;
using ShardingConnector.ParserBinder.Command;
using ShardingConnector.ParserBinder.MetaData.Schema;
using ShardingConnector.ShardingCommon.Core.Rule;

namespace ShardingConnector.ShardingMerge.DAL.Show
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/5/5 10:48:06
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public class LogicTablesMergedEnumerator:MemoryMergedEnumerator<ShardingRule>
    {
        public LogicTablesMergedEnumerator(ShardingRule rule, SchemaMetaData schemaMetaData, ISqlCommandContext<ISqlCommand> sqlCommandContext, List<IQueryEnumerator> queryEnumerators) : base(rule, schemaMetaData, sqlCommandContext, queryEnumerators)
        {
        }

        protected override List<MemoryQueryResultRow> Init(ShardingRule rule, SchemaMetaData schemaMetaData, ISqlCommandContext<ISqlCommand> sqlCommandContext,
            List<IQueryEnumerator> queryEnumerators)
        {
            throw new System.NotImplementedException();
        }
    }
}
