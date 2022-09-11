using NCDC.Sharding.Rewrites.Sql.Token.SimpleObject;

namespace NCDC.ShardingRewrite.Token.SimpleObject
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/27 16:14:19
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public abstract class GeneratedKeyAssignmentToken:SqlToken,IAttachable
    {
        private readonly string _columnName;
        public GeneratedKeyAssignmentToken(int startIndex, string columnName) : base(startIndex)
        {
            _columnName = columnName;
        }

        public override string ToString()
        {
            return $", {_columnName} = {GetRightValue()}";
        }

        protected abstract string GetRightValue();
    }
}
