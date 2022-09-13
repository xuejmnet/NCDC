using NCDC.Basic.TableMetadataManagers;
using NCDC.CommandParser.Abstractions;
using NCDC.ShardingParser.Command;
using NCDC.StreamDataReaders;

namespace NCDC.ShardingMerge.DataReaders.Memory
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
    public abstract class MemoryMergedDataReader : IStreamDataReader
    {
        private readonly IEnumerator<MemoryQueryResultRow> _memoryResultSetRows;

        private MemoryQueryResultRow _currentSetResultRow;


        protected MemoryMergedDataReader(ITableMetadataManager tableMetadataManager,
            ISqlCommandContext<ISqlCommand> sqlCommandContext, List<IStreamDataReader> streamDataReaders)
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            var memoryQueryResultRowList = Init(tableMetadataManager, sqlCommandContext, streamDataReaders);
            _memoryResultSetRows = memoryQueryResultRowList.GetEnumerator();
            if (memoryQueryResultRowList.Any())
            {
                _currentSetResultRow = memoryQueryResultRowList.First();
            }
        }

        protected abstract List<MemoryQueryResultRow> Init(ITableMetadataManager tableMetadataManager,
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

        public void Dispose()
        {
            _memoryResultSetRows?.Dispose();
            _currentSetResultRow?.Dispose();
        }
    }
}