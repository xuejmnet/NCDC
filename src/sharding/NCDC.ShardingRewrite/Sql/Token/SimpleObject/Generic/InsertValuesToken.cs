namespace NCDC.ShardingRewrite.Sql.Token.SimpleObject.Generic
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/24 13:38:23
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public abstract class InsertValuesToken:SqlToken,ISubstitutable
    {
        private readonly int _stopIndex;

        public readonly List<InsertValue> InsertValues = new List<InsertValue>();

        public InsertValuesToken(int startIndex, int stopIndex) : base(startIndex)
        {
            this._stopIndex = stopIndex;
        }

        public int GetStopIndex()
        {
            return _stopIndex;
        }
    }
}
