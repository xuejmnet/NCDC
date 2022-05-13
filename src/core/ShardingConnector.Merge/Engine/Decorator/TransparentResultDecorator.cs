using System;
using System.Collections.Generic;
using System.Text;

using ShardingConnector.CommandParser.Command;
using ShardingConnector.Executor;
using ShardingConnector.Merge.Reader;
using ShardingConnector.Merge.Reader.Transparent;
using ShardingConnector.ParserBinder.Command;
using ShardingConnector.ParserBinder.MetaData.Schema;

namespace ShardingConnector.Merge.Engine.Decorator
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/25 10:45:42
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class TransparentResultDecorator:IResultDecorator
    {
        public IMergedDataReader Decorate(IQueryDataReader queryResult, ISqlCommandContext<ISqlCommand> sqlCommandContext,
            SchemaMetaData schemaMetaData)
        {
            return new TransparentMergedDataReader(queryResult);
        }

        public IMergedDataReader Decorate(IMergedDataReader mergedResult, ISqlCommandContext<ISqlCommand> sqlCommandContext,
            SchemaMetaData schemaMetaData)
        {
            return mergedResult;
        }
    }
}
