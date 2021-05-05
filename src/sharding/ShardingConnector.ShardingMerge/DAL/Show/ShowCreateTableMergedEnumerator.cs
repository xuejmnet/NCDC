using System;
using System.Collections.Generic;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.Executor;
using ShardingConnector.Extensions;
using ShardingConnector.Merge.Reader.Memory;
using ShardingConnector.ParserBinder.Command;
using ShardingConnector.ParserBinder.MetaData.Schema;
using ShardingConnector.ShardingCommon.Core.Rule;

namespace ShardingConnector.ShardingMerge.DAL.Show
{
/*
* @Author: xjm
* @Description:
* @Date: Wednesday, 05 May 2021 20:22:04
* @Email: 326308290@qq.com
*/
    public sealed class ShowCreateTableMergedEnumerator:LogicTablesMergedEnumerator
    {
        public ShowCreateTableMergedEnumerator(ShardingRule rule, SchemaMetaData schemaMetaData, ISqlCommandContext<ISqlCommand> sqlCommandContext, List<IQueryEnumerator> queryEnumerators) : base(rule, schemaMetaData, sqlCommandContext, queryEnumerators)
        {
        }

        protected override void SetCellValue(MemoryQueryResultRow memoryResultSetRow, string logicTableName, string actualTableName)
        {  
            memoryResultSetRow.SetCell(2, memoryResultSetRow.GetCell(2).ToString().ReplaceFirst(actualTableName, logicTableName));
        }
    }
}