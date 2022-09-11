using NCDC.Sharding.Rewrites.Sql.Token.SimpleObject;

namespace NCDC.Sharding.Rewrites.Token.SimpleObject
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/27 16:29:49
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class OffsetToken:SqlToken,ISubstitutable
    {
        private readonly int stopIndex;

        private readonly long revisedOffset;
        public OffsetToken(int startIndex, int stopIndex, long revisedOffset) : base(startIndex)
        {
            this.stopIndex = stopIndex;
            this.revisedOffset = revisedOffset;
        }

        public int GetStopIndex()
        {
            return stopIndex;
        }

        public override string ToString()
        {
            return $"{revisedOffset}";
        }
    }
}
