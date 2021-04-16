using System.Collections.Generic;
using System.Linq;
using ShardingConnector.Common.Rule;
using ShardingConnector.Executor;
using ShardingConnector.Extensions;
using ShardingConnector.Kernels.MetaData.Schema;
using ShardingConnector.Parser.Binder.Command;
using ShardingConnector.Parser.Sql.Command;

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
        private readonly IEnumerator<MemoryQueryRow> _memoryResultSetRows;

        private MemoryQueryRow _currentSetRow;


        protected MemoryMergedEnumerator(T rule, SchemaMetaData schemaMetaData,
            ISqlCommandContext<ISqlCommand> sqlCommandContext, List<IQueryEnumerator> queryEnumerators)
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            var memoryQueryResultRowList = Init(rule, schemaMetaData, sqlCommandContext, queryEnumerators);
            _memoryResultSetRows = memoryQueryResultRowList.GetEnumerator();
            if (memoryQueryResultRowList.Any())
            {
                _currentSetRow = memoryQueryResultRowList.First();
            }
        }

        protected abstract List<MemoryQueryRow> Init(T rule, SchemaMetaData schemaMetaData,
            ISqlCommandContext<ISqlCommand> sqlCommandContext, List<IQueryEnumerator> queryEnumerators);

        public bool MoveNext()
        {
            if (_memoryResultSetRows.MoveNext())
            {
                _currentSetRow = _memoryResultSetRows.Current;
                return true;
            }

            return false;
        }

        public object GetValue(int columnIndex)
        {
            object result = _currentSetRow.GetCell(columnIndex);
            return result;
        }

        public T1 GetValue<T1>(int columnIndex)
        {
            object result = _currentSetRow.GetCell(columnIndex);
            return (T1)result;
        }

        public bool IsDBNull(int columnIndex)
        {
            return _currentSetRow.IsDBNull(columnIndex);
        }

    }
}