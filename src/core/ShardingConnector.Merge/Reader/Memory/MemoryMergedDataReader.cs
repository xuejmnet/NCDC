using System;
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
            var memoryQueryResultRowList = Init(rule, schemaMetaData, sqlCommandContext, streamDataReaders);
            _memoryResultSetRows = memoryQueryResultRowList.GetEnumerator();
            if (memoryQueryResultRowList.Any())
            {
                _currentSetResultRow = memoryQueryResultRowList.First();
            }
        }

        protected abstract List<MemoryQueryResultRow> Init(T rule, SchemaMetaData schemaMetaData,
            ISqlCommandContext<ISqlCommand> sqlCommandContext, List<IStreamDataReader> queryEnumerators);

        public bool Read()
        {
            if (_memoryResultSetRows.MoveNext())
            {
                _currentSetResultRow = _memoryResultSetRows.Current;
                return true;
            }

            return false;
        }

        public int ColumnCount { get; }
        public string GetColumnName(int columnIndex)
        {
            throw new NotImplementedException();
        }

        public string GetColumnLabel(int columnIndex)
        {
            throw new NotImplementedException();
        }

        public object this[int columnIndex] => _currentSetResultRow.GetCell(columnIndex);

        public object this[string name] => _currentSetResultRow.GetCell(_currentSetResultRow.GetOrdinal(name));

        public string GetName(int columnIndex)
        {
            throw new NotImplementedException();
        }

        public string GetDataTypeName(int columnIndex)
        {
            throw new NotImplementedException();
        }

        public Type GetFieldType(int columnIndex)
        {
            throw new NotImplementedException();
        }

        public object GetValue(int columnIndex)
        {
            object result = _currentSetResultRow.GetCell(columnIndex);
            return result;
        }

        public int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public int GetOrdinal(string name)
        {
            throw new NotImplementedException();
        }

        public bool GetBoolean(int columnIndex)
        {
            throw new NotImplementedException();
        }

        public byte GetByte(int columnIndex)
        {
            throw new NotImplementedException();
        }

        public long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
        {
            throw new System.NotImplementedException();
        }

        public char GetChar(int columnIndex)
        {
            throw new NotImplementedException();
        }

        public long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
        {
            throw new System.NotImplementedException();
        }

        public Guid GetGuid(int columnIndex)
        {
            throw new NotImplementedException();
        }

        public short GetInt16(int columnIndex)
        {
            throw new NotImplementedException();
        }

        public int GetInt32(int columnIndex)
        {
            throw new NotImplementedException();
        }

        public long GetInt64(int columnIndex)
        {
            throw new NotImplementedException();
        }

        public float GetFloat(int columnIndex)
        {
            throw new NotImplementedException();
        }

        public double GetDouble(int columnIndex)
        {
            throw new NotImplementedException();
        }

        public string GetString(int columnIndex)
        {
            throw new NotImplementedException();
        }

        public decimal GetDecimal(int columnIndex)
        {
            throw new NotImplementedException();
        }

        public DateTime GetDateTime(int columnIndex)
        {
            throw new NotImplementedException();
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