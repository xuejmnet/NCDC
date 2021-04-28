using System;
using System.Collections.Generic;
using System.Text;

namespace ShardingConnector.ShardingRoute.Engine.Condition
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/28 12:06:42
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class Column
    {
        public Column(string name, string tableName)
        {
            Name = name;
            TableName = tableName;
        }

        public  string Name { get; }

        public  string TableName { get; }

        private bool Equals(Column other)
        {
            return Name == other.Name && TableName == other.TableName;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is Column other && Equals(other);
        }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(TableName)}: {TableName}";
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ (TableName != null ? TableName.GetHashCode() : 0);
            }
        }
    }
}
