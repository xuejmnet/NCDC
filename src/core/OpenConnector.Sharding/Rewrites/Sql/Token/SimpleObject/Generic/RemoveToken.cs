namespace OpenConnector.Sharding.Rewrites.Sql.Token.SimpleObject.Generic
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/24 10:44:07
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class RemoveToken:SqlToken,ISubstitutable
    {
        private readonly int _stopIndex;

        public RemoveToken(int startIndex,int stopIndex) : base(startIndex)
        {
            _stopIndex = stopIndex;
        }

        public int GetStopIndex()
        {
            return _stopIndex;
        }
    }
}
