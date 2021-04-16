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
    public sealed class MemoryQueryRow
    {
        private readonly object[] data;

        public MemoryQueryRow(IQueryEnumerator queryEnumerator)
        {
            data = Load(queryEnumerator);
        }

        private object[] Load(IQueryEnumerator queryEnumerator)
        {
            int columnCount = queryEnumerator.ColumnCount;
            object[] result = new object[columnCount];
            for (int i = 0; i < columnCount; i++)
            {
                result[i] = queryEnumerator.GetValue(i + 1);
            }

            return result;
        }

        /**
         * Get data from cell.
         * 
         * @param columnIndex column index
         * @return data from cell
         */
        public object GetCell(int columnIndex)
        {
            if (columnIndex <= 0 || columnIndex >= (data.Length + 1))
            {
                throw new ShardingException($"Get Cell {columnIndex}");
            }

            return data[columnIndex - 1];
        }

        /**
         * Set data for cell.
         *
         * @param columnIndex column index
         * @param value data for cell
         */
        public void SetCell(int columnIndex, object value)
        {
            if (columnIndex <= 0 || columnIndex >= (data.Length + 1))
            {
                throw new ShardingException($"Set Cell {columnIndex}");
            }

            data[columnIndex - 1] = value;
        }

        public bool IsDBNull(int columnIndex)
        {
            if (columnIndex <= 0 || columnIndex >= (data.Length + 1))
            {
                throw new ShardingException($"Get Cell {columnIndex}");
            }

            return null==data[columnIndex];
        }
    }
}