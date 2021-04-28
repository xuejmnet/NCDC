using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShardingConnector.Exceptions;

namespace ShardingConnector.Common.Rule
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/13 16:02:51
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class DataNode
    {

        private static readonly char DELIMITER = '.';

        private readonly string _dataSourceName;

        private readonly string _tableName;

        /**
     * Constructs a data node with well-formatted string.
     *
     * @param dataNode string of data node. use {@code .} to split data source name and table name.
     */
        public DataNode(string dataNode)
        {
            if (!IsValidDataNode(dataNode))
            {
                throw new ShardingException($"Invalid format for actual data nodes: '{dataNode}'");
            }

            var segments = dataNode.Split(DELIMITER).ToList();
            _dataSourceName = segments[0];
            _tableName = segments[1];
        }

        public DataNode(string dataSourceName, string tableName)
        {
            _dataSourceName = dataSourceName;
            _tableName = tableName;
        }

        private static bool IsValidDataNode(string dataNodeStr)
        {
            return dataNodeStr.Contains(DELIMITER.ToString()) && 2 == dataNodeStr.Split(DELIMITER).Length;
        }

        public string GetTableName()
        {
            return _tableName;
        }
        public string GetDataSourceName()
        {
            return _dataSourceName;
        }

        //public boolean equals(final Object object)
        //{
        //    if (this == object)
        //    {
        //        return true;
        //    }
        //    if (null == object || getClass() != object.getClass())
        //    {
        //        return false;
        //    }
        //    DataNode dataNode = (DataNode)object;
        //    return Objects.equal(this.dataSourceName.toUpperCase(), dataNode.dataSourceName.toUpperCase())
        //           && Objects.equal(this.tableName.toUpperCase(), dataNode.tableName.toUpperCase());
        //}

        //public int hashCode()
        //{
        //    return Objects.hashCode(dataSourceName.toUpperCase(), tableName.toUpperCase());
        //}

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            if (null == obj)
            {
                return false;
            }
            DataNode dataNode = (DataNode)obj;
            return object.Equals(this._dataSourceName.ToUpper(), dataNode._dataSourceName.ToUpper())
                   && object.Equals(this._tableName.ToUpper(), dataNode._tableName.ToUpper());
        }

        public override string ToString()
        {
            return $"{nameof(_dataSourceName)}: {_dataSourceName}, {nameof(_tableName)}: {_tableName}";
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((_dataSourceName != null ? _dataSourceName.GetHashCode() : 0) * 397) ^ (_tableName != null ? _tableName.GetHashCode() : 0);
            }
        }
    }
}
