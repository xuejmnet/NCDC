using System.Collections.Generic;
using ShardingConnector.Exceptions;
using ShardingConnector.Executor;

namespace ShardingConnector.Merge.Reader.Memory
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
    public sealed class MemoryQueryResultRow
    {
        private readonly object[] _data;
        private readonly Dictionary<string,int> _columns;

        public MemoryQueryResultRow(IStreamDataReader streamDataReader)
        {
            var (data,columns) = Load(streamDataReader);
            _data = data;
            _columns = columns;
        }

        private (object[] data, Dictionary<string, int> columns) Load(IQueryDataReader queryDataReader)
        {
            int columnCount = queryDataReader.ColumnCount;
            object[] result = new object[columnCount];
            var columns = new Dictionary<string, int>(columnCount);
            for (int i = 0; i < columnCount; i++)
            {
                result[i] = queryDataReader.GetValue(i);
                var columnName = queryDataReader.GetColumnName(i);
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
        public object GetCell(string columnName)
        {
            if (!_columns.ContainsKey(columnName))
            {
                throw new ShardingException($"Get Cell {columnName}");
            }

            var columnIndex = _columns[columnName];
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
        public void SetCell(string columnName, object value)
        {
            if (!_columns.ContainsKey(columnName))
            {
                throw new ShardingException($"Set Cell {columnName}");
            }

            var columnIndex = _columns[columnName];
            _data[columnIndex] = value;
        }

        public bool IsDBNull(int columnIndex)
        {
            if (columnIndex < 0 || columnIndex >= _data.Length)
            {
                throw new ShardingException($"Get Cell {columnIndex}");
            }

            return null==_data[columnIndex];
        }
    }
}