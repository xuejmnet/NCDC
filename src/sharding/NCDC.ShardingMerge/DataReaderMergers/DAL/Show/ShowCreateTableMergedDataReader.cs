using NCDC.CommandParser.Abstractions;
using NCDC.Extensions;
using NCDC.ShardingMerge.DataReaders.Memory;
using NCDC.ShardingParser.Command;
using NCDC.ShardingParser.MetaData.Schema;
using NCDC.StreamDataReaders;

namespace NCDC.ShardingMerge.DataReaderMergers.DAL.Show
{
/*
* @Author: xjm
* @Description:
* @Date: Wednesday, 05 May 2021 20:22:04
* @Email: 326308290@qq.com
*/
    public sealed class ShowCreateTableMergedDataReader:LogicTablesMergedDataReader
    {
        public ShowCreateTableMergedDataReader(SchemaMetaData schemaMetaData, ISqlCommandContext<ISqlCommand> sqlCommandContext, List<IStreamDataReader> streamDataReaders) : base(schemaMetaData, sqlCommandContext, streamDataReaders)
        {
        }

        protected override void SetCellValue(MemoryQueryResultRow memoryResultSetRow, string logicTableName, string actualTableName)
        {  
            memoryResultSetRow.SetCell(2, memoryResultSetRow.GetCell(2).ToString().ReplaceFirst(actualTableName, logicTableName));
        }
    }
}