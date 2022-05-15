using ShardingConnector.CommandParser.Command;
using ShardingConnector.Common.Rule;
using ShardingConnector.Executor;
using ShardingConnector.ParserBinder.Command;
using ShardingConnector.ParserBinder.MetaData.Schema;
using System.Collections.Generic;
using System.Linq;


namespace ShardingConnector.Merge.Reader.Memory
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/16 00:00:00
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    /// <summary>
    /// 
    /// </summary>
    public abstract class MemoryMergedDataReader<T> : IStreamDataReader where T : IBaseRule
    {
        private readonly IEnumerator<MemoryQueryResultRow> _memoryResultSetRows;

        private MemoryQueryResultRow _currentSetResultRow;


        protected MemoryMergedDataReader(T rule, SchemaMetaData schemaMetaData,
            ISqlCommandContext<ISqlCommand> sqlCommandContext, List<IStreamDataReader> streamDataReaders)
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            var memoryQueryResultRowList = Init(rule, schemaMetaData, sqlCommandContext, queryDataReaders);
            _memoryResultSetRows = memoryQueryResultRowList.GetEnumerator();
            if (memoryQueryResultRowList.Any())
            {
                _currentSetResultRow = memoryQueryResultRowList.First();
            }
        }

        protected abstract List<MemoryQueryResultRow> Init(T rule, SchemaMetaData schemaMetaData,
            ISqlCommandContext<ISqlCommand> sqlCommandContext, List<IQueryDataReader> queryEnumerators);

        public bool Read()
        {
            if (_memoryResultSetRows.MoveNext())
            {
                _currentSetResultRow = _memoryResultSetRows.Current;
                return true;
            }

            return false;
        }

        public object GetValue(int columnIndex)
        {
            object result = _currentSetResultRow.GetCell(columnIndex);
            return result;
        }

        public T1 GetValue<T1>(int columnIndex)
        {
            object result = _currentSetResultRow.GetCell(columnIndex);
            return (T1)result;
        }

        public object GetValue(string columnName)
        {
            return _currentSetResultRow.GetCell(columnName);
        }

        public T1 GetValue<T1>(string columnName)
        {
            return (T1)_currentSetResultRow.GetCell(columnName);
        }

        public long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
        {
            throw new System.NotImplementedException();
        }

        public long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
        {
            throw new System.NotImplementedException();
        }

        public bool IsDBNull(int columnIndex)
        {
            return _currentSetResultRow.IsDBNull(columnIndex);
        }

        public bool NextResult()
        {
            throw new System.NotImplementedException();
        }
    }
}