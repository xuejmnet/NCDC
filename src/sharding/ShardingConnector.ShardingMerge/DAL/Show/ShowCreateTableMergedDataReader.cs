using System;
using System.Collections.Generic;

using ShardingConnector.CommandParser.Command;
using ShardingConnector.Extensions;
using ShardingConnector.Merge.Reader.Memory;
using ShardingConnector.CommandParserBinder.Command;
using ShardingConnector.CommandParserBinder.MetaData.Schema;
using ShardingConnector.ShardingCommon.Core.Rule;

namespace ShardingConnector.ShardingMerge.DAL.Show
{
/*
* @Author: xjm
* @Description:
* @Date: Wednesday, 05 May 2021 20:22:04
* @Email: 326308290@qq.com
*/
    public sealed class ShowCreateTableMergedDataReader:LogicTablesMergedDataReader
    {
        public ShowCreateTableMergedDataReader(ShardingRule rule, SchemaMetaData schemaMetaData, ISqlCommandContext<ISqlCommand> sqlCommandContext, List<IStreamDataReader> streamDataReaders) : base(rule, schemaMetaData, sqlCommandContext, streamDataReaders)
        {
        }

        protected override void SetCellValue(MemoryQueryResultRow memoryResultSetRow, string logicTableName, string actualTableName)
        {  
            memoryResultSetRow.SetCell(2, memoryResultSetRow.GetCell(2).ToString().ReplaceFirst(actualTableName, logicTableName));
        }
    }
}