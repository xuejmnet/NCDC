using System;
using System.Collections.Generic;
using System.Text;

namespace ShardingConnector.RewriteEngine.Sql.Token.SimpleObject.Generic
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/24 13:40:32
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class SubstitutableColumnNameToken:SqlToken,ISubstitutable
    {
        private readonly int _stopIndex;

        private readonly string _columnName;
    
        public SubstitutableColumnNameToken(int startIndex, int stopIndex, string columnName) : base(startIndex)
        {
            _stopIndex = stopIndex;
            _columnName = columnName;
        }

        public int GetStopIndex()
        {
            return _stopIndex;
        }

        public string ColumnName()
        {
            return _columnName;
        }

        private bool Equals(SubstitutableColumnNameToken other)
        {
            return _stopIndex == other._stopIndex && _columnName == other._columnName;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is SubstitutableColumnNameToken other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_stopIndex * 397) ^ (_columnName != null ? _columnName.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return _columnName;
        }
    }
}
