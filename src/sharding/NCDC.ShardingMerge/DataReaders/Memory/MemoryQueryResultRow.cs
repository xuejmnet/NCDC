using NCDC.Exceptions;
using NCDC.StreamDataReaders;

namespace NCDC.ShardingMerge.DataReaders.Memory
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/16 16:55:00
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public sealed class MemoryQueryResultRow:IDisposable
    {
        private readonly IStreamDataReader _streamDataReader;
        private readonly object[] _data;
        private readonly Dictionary<string, int> _columns;

        public MemoryQueryResultRow(IStreamDataReader streamDataReader)
        {
            _streamDataReader = streamDataReader;
            var (data, columns) = Load(streamDataReader);
            _data = data;
            _columns = columns;
        }

        private (object[] data, Dictionary<string, int> columns) Load(IStreamDataReader streamDataReader)
        {
            int columnCount = streamDataReader.ColumnCount;
            object[] result = new object[columnCount];
            var columns = new Dictionary<string, int>(columnCount);
            for (int i = 0; i < columnCount; i++)
            {
                result[i] = streamDataReader.GetValue(i);
                var columnName = streamDataReader.GetColumnName(i);
                columns[columnName] = i;
            }

            return (result, columns);
        }

        /**
         * Get _data from cell.
         * 
         * @param columnIndex column index
         * @return _data from cell
         */
        public object GetCell(int columnIndex)
        {
            if (columnIndex < 0 || columnIndex >= _data.Length)
            {
                throw new ShardingException($"Get Cell {columnIndex}");
            }

            return _data[columnIndex];
        }

        /**
         * Set _data for cell.
         *
         * @param columnIndex column index
         * @param value _data for cell
         */
        public void SetCell(int columnIndex, object value)
        {
            if (columnIndex < 0 || columnIndex >= _data.Length)
            {
                throw new ShardingException($"Set Cell {columnIndex}");
            }

            _data[columnIndex] = value;
        }

        public int GetOrdinal(string columnName)
        {
            if (!_columns.ContainsKey(columnName))
            {
                throw new ShardingException($"Get Ordinal {columnName}");
            }

            return _columns[columnName];
        }
        public bool IsDBNull(int columnIndex)
        {
            if (columnIndex < 0 || columnIndex >= _data.Length)
            {
                throw new ShardingException($"Get Cell {columnIndex}");
            }

            return null == _data[columnIndex];
        }

        public void Dispose()
        {
            _streamDataReader?.Dispose();
        }
    }
}