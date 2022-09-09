using OpenConnector.Sharding.Rewrites.Sql.Token.SimpleObject;

namespace OpenConnector.Sharding.Rewrites.Token.SimpleObject
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/27 16:09:56
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class AggregationDistinctToken:SqlToken,ISubstitutable
    {
        private readonly string _columnName;
    
        private readonly string _derivedAlias;
    
        
        private readonly int _stopIndex;
        public AggregationDistinctToken(int startIndex, int stopIndex, string columnName, string derivedAlias) : base(startIndex)
        {
            this._columnName = columnName;
            this._derivedAlias = derivedAlias;
            this._stopIndex = stopIndex;
        }

        public int GetStopIndex()
        {
            return _stopIndex;
        }

        public override string ToString()
        {
            return null == _derivedAlias ? _columnName : _columnName + " AS " + _derivedAlias;
        }
    }
}
