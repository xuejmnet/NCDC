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
    public abstract class MemoryMergedEnumerator<T> : IMergedEnumerator where T : IBaseRule
    {
        private readonly IEnumerator<MemoryQueryResultRow> _memoryResultSetRows;

        private MemoryQueryResultRow _currentSetResultRow;


        protected MemoryMergedEnumerator(T rule, SchemaMetaData schemaMetaData,
            ISqlCommandContext<ISqlCommand> sqlCommandContext, List<IQueryEnumerator> queryEnumerators)
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            var memoryQueryResultRowList = Init(rule, schemaMetaData, sqlCommandContext, queryEnumerators);
            _memoryResultSetRows = memoryQueryResultRowList.GetEnumerator();
            if (memoryQueryResultRowList.Any())
            {
                _currentSetResultRow = memoryQueryResultRowList.First();
            }
        }

        protected abstract List<MemoryQueryResultRow> Init(T rule, SchemaMetaData schemaMetaData,
            ISqlCommandContext<ISqlCommand> sqlCommandContext, List<IQueryEnumerator> queryEnumerators);

        public bool MoveNext()
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

        public bool IsDBNull(int columnIndex)
        {
            return _currentSetResultRow.IsDBNull(columnIndex);
        }

    }
}