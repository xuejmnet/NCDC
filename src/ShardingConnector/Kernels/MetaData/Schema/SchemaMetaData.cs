using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ShardingConnector.Kernels.MetaData.Table;

namespace ShardingConnector.Kernels.MetaData.Schema
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: Thursday, 08 April 2021 21:54:32
    * @Email: 326308290@qq.com
    */
    public sealed class SchemaMetaData
    {

        private readonly ConcurrentDictionary<string, TableMetaData> _tables = new ConcurrentDictionary<string, TableMetaData>();

        public SchemaMetaData(Dictionary<String, TableMetaData> tables)
        {
            foreach (var tableEntry in tables)
            {
                _tables.TryAdd(tableEntry.Key, tableEntry.Value);
            }
        }

        /**
         * Get all table names.
         *
         * @return all table names
         */
        public ICollection<string> GetAllTableNames()
        {
            return _tables.Keys;
        }

        /**
         * Get table meta data via table name.
         * 
         * @param tableName tableName table name
         * @return table mata data
         */
        public TableMetaData Get(string tableName)
        {
            if (_tables.TryGetValue(tableName.ToLower(), out var result))
            {
                return result;
            }
            return null;
        }

        /**
         * Merge schema meta data.
         * 
         * @param schemaMetaData schema meta data
         */
        public void Merge(SchemaMetaData schemaMetaData)
        {
            foreach (var tableMetaData in schemaMetaData._tables)
            {
                _tables.TryAdd(tableMetaData.Key, tableMetaData.Value);
            }
        }

        /**
         * Add table meta data.
         * 
         * @param tableName table name
         * @param tableMetaData table meta data
         */
        public void Add(string tableName, TableMetaData tableMetaData)
        {
            _tables.TryAdd(tableName.ToLower(), tableMetaData);
        }

        /**
         * Remove table meta data.
         *
         * @param tableName table name
         */
        public void Remove(string tableName)
        {
            _tables.TryRemove(tableName.ToLower(), out var v);
        }

        /**
         * Judge contains table from table meta data or not.
         *
         * @param tableName table name
         * @return contains table from table meta data or not
         */
        public bool ContainsTable(string tableName)
        {
            return _tables.ContainsKey(tableName.ToLower());
        }

        /**
         * Judge whether contains column name.
         *
         * @param tableName table name
         * @param columnName column name
         * @return contains column name or not
         */
        public bool ContainsColumn(string tableName, string columnName)
        {
            return ContainsTable(tableName) && Get(tableName).GetColumns().ContainsKey(columnName.ToLower());
        }

        /**
         * Get all column names via table.
         *
         * @param tableName table name
         * @return column names
         */
        public List<string> GetAllColumnNames(string tableName)
        {
            return ContainsTable(tableName) ? new List<string>(Get(tableName).GetColumns().Keys) : new List<string>(0);
        }
    }
}