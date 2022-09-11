using NCDC.Sharding.Rewrites.Sql.Token.SimpleObject;

namespace NCDC.Sharding.Rewrites.Token.SimpleObject
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/27 16:45:27
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class RowCountToken:SqlToken,ISubstitutable
    {
        private readonly int stopIndex;

        private readonly long revisedRowCount;
        public RowCountToken(int startIndex, int stopIndex, long revisedRowCount) : base(startIndex)
        {
            this.stopIndex = stopIndex;
            this.revisedRowCount = revisedRowCount;
        }

        public int GetStopIndex()
        {
            return stopIndex;
        }

        public override string ToString()
        {
            return $"{revisedRowCount}";
        }
    }
}
