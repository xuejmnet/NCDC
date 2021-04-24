using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ShardingConnector.ParserBinder.MetaData.Column;
using ShardingConnector.ParserBinder.MetaData.Index;

namespace ShardingConnector.ParserBinder.MetaData.Table
{
/*
* @Author: xjm
* @Description:
* @Date: Thursday, 08 April 2021 21:55:23
* @Email: 326308290@qq.com
*/
    public sealed class TableMetaData
    {
        private readonly ConcurrentDictionary<string, ColumnMetaData> _columns;

        private readonly ConcurrentDictionary<string, IndexMetaData> _indexes;

        private readonly List<string> _columnNames = new List<string>();

        private readonly List<string> _primaryKeyColumns = new List<string>();

        public TableMetaData(ICollection<ColumnMetaData> columnMetaDataList, ICollection<IndexMetaData> indexMetaDataList)
        {
            _columns = CreateColumns(columnMetaDataList);
            _indexes = CreateIndexes(indexMetaDataList);
        }

        private ConcurrentDictionary<string, ColumnMetaData> CreateColumns(ICollection<ColumnMetaData> columnMetaDataList)
        {
            ConcurrentDictionary<string, ColumnMetaData> result = new ConcurrentDictionary<string, ColumnMetaData>();
            foreach (var columnMetaData in columnMetaDataList)
            {
                var lowerColumnName = columnMetaData.Name.ToLower();
                _columnNames.Add(lowerColumnName);
                result.TryAdd(lowerColumnName, columnMetaData);
                if (columnMetaData.PrimaryKey)
                {
                    _primaryKeyColumns.Add(lowerColumnName);
                }
            }

            return result;
        }

        private bool Equals(TableMetaData other)
        {
            return Equals(_columns, other._columns) && Equals(_indexes, other._indexes) && Equals(_columnNames, other._columnNames) && Equals(_primaryKeyColumns, other._primaryKeyColumns);
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is TableMetaData other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (_columns != null ? _columns.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_indexes != null ? _indexes.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_columnNames != null ? _columnNames.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_primaryKeyColumns != null ? _primaryKeyColumns.GetHashCode() : 0);
                return hashCode;
            }
        }

        private ConcurrentDictionary<string, IndexMetaData> CreateIndexes(ICollection<IndexMetaData> indexMetaDataList)
        {
            ConcurrentDictionary<string, IndexMetaData> result = new ConcurrentDictionary<string, IndexMetaData>();
            foreach (var indexMetaData in indexMetaDataList)
            {
                result.TryAdd(indexMetaData.Name.ToLower(), indexMetaData);
            }

            return result;
        }

        public ConcurrentDictionary<string, ColumnMetaData> GetColumns()
        {
            return _columns;
        }

        public ConcurrentDictionary<string, IndexMetaData> GetIndexes()
        {
            return _indexes;
        }
        /**
     * Get column meta data.
     *
     * @param columnIndex column index
     * @return column meta data
     */
        public ColumnMetaData GetColumnMetaData(int columnIndex)
        {
            if (_columns.TryGetValue(_columnNames[columnIndex], out var value))
            {
                return value;
            }

            return null;
        }

        /**
     * Find index of column.
     *
     * @param columnName column name
     * @return index of column if found, otherwise -1
     */
        public int FindColumnIndex(string columnName)
        {
            for (int i = 0; i < _columnNames.Count; i++)
            {
                if (_columnNames[i].Equals(columnName))
                {
                    return i;
                }
            }

            return -1;
        }

        /**
     * Judge column whether primary key.
     *
     * @param columnIndex column index
     * @return true if the column is primary key, otherwise false
     */
        public bool IsPrimaryKey(int columnIndex)
        {
            if (columnIndex >= _columnNames.Count)
            {
                return false;
            }

            if (_columns.TryGetValue(_columnNames[columnIndex], out var value))
            {
                return value.PrimaryKey;
            }

            return false;
        }
    }
}