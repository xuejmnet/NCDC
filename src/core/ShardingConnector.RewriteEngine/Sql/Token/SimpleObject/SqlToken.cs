using System;

namespace ShardingConnector.RewriteEngine.Sql.Token.SimpleObject
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 16:05:38
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public abstract class SqlToken:IComparable<SqlToken>
    {
        private readonly int _startIndex;

        public SqlToken(int startIndex)
        {
            _startIndex = startIndex;
        }
        public int CompareTo(SqlToken sqlToken)
        {
            return _startIndex - sqlToken.GetStartIndex();
        }

        public int GetStartIndex()
        {
            return _startIndex;
        }
    }
}
