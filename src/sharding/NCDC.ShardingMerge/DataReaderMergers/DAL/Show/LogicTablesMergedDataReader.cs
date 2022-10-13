using NCDC.Basic.TableMetadataManagers;
using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Common.Command;
using NCDC.ShardingMerge.DataReaders.Memory;
using NCDC.ShardingParser.Command;
using NCDC.StreamDataReaders;

namespace NCDC.ShardingMerge.DataReaderMergers.DAL.Show
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/5/5 10:48:06
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public class LogicTablesMergedDataReader:MemoryMergedDataReader
    {
        public LogicTablesMergedDataReader(ITableMetadataManager tableMetadataManager, ISqlCommandContext<ISqlCommand> sqlCommandContext, List<IStreamDataReader> streamDataReaders) : base(tableMetadataManager, sqlCommandContext, streamDataReaders)
        {
        }

        protected override List<MemoryQueryResultRow> Init(ITableMetadataManager tableMetadataManager, ISqlCommandContext<ISqlCommand> sqlCommandContext,
            List<IStreamDataReader> streamDataReaders)
        {
            ICollection<MemoryQueryResultRow> result = new LinkedList<MemoryQueryResultRow>();
            var tableNames = new HashSet<string>();
            foreach (var streamDataReader in streamDataReaders)
            {
                while (streamDataReader.Read()) {
                    MemoryQueryResultRow memoryResultSetRow = new MemoryQueryResultRow(streamDataReader);
                    var actualTableName = memoryResultSetRow.GetCell(0).ToString();
                    if (actualTableName != null)
                    { 
                        var tableMetadata = tableMetadataManager.TryGetByActualTableName(actualTableName);
                        if (tableMetadata==null||!tableMetadata.IsSharding) {
                            if (tableMetadataManager.Contains(actualTableName) && tableNames.Add(actualTableName)) {
                                result.Add(memoryResultSetRow);
                            }
                        } else if (tableNames.Add(tableMetadata.LogicTableName)) {
                            memoryResultSetRow.SetCell(1, tableMetadata.LogicTableName);
                            SetCellValue(memoryResultSetRow, tableMetadata.LogicTableName, actualTableName);
                            result.Add(memoryResultSetRow);
                        }
                    }
                }
            }
            return result.ToList();
        }
    
        protected virtual void SetCellValue( MemoryQueryResultRow memoryResultSetRow,  string logicTableName,  string actualTableName) {
        }
    }
}
