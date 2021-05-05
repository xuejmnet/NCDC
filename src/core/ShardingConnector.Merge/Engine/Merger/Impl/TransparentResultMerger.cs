using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.Executor;
using ShardingConnector.Merge.Reader;
using ShardingConnector.Merge.Reader.Transparent;
using ShardingConnector.ParserBinder.Command;
using ShardingConnector.ParserBinder.MetaData.Schema;

namespace ShardingConnector.Merge.Engine.Merger.Impl
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/5/5 10:15:26
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class TransparentResultMerger:IResultMerger
    {
        public IMergedEnumerator Merge(List<IQueryEnumerator> queryResults, ISqlCommandContext<ISqlCommand> sqlCommandContext, SchemaMetaData schemaMetaData)
        {
            return new TransparentMergedEnumerator(queryResults[0]);
        }
    }
}
