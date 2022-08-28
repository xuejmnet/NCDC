﻿using System;
using System.Collections.Generic;
using System.Text;

using ShardingConnector.CommandParser.Command;
using ShardingConnector.CommandParserBinder.Command;
using ShardingConnector.CommandParserBinder.MetaData.Schema;
using ShardingConnector.Merge.Reader.Transparent;

namespace ShardingConnector.Merge.Engine.Decorator
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/25 10:45:42
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class TransparentDataReaderDecorator:IDataReaderDecorator
    {
        public IStreamDataReader Decorate(IStreamDataReader streamDataReader, ISqlCommandContext<ISqlCommand> sqlCommandContext,
            SchemaMetaData schemaMetaData)
        {
            return new TransparentMergedDataReader(streamDataReader);
        }
    }
}
