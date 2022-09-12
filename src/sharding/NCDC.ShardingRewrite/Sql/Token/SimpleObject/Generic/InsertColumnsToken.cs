using NCDC.Extensions;

namespace NCDC.ShardingRewrite.Sql.Token.SimpleObject.Generic
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/24 10:45:32
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class InsertColumnsToken:SqlToken, IAttachable
    {
        private readonly List<string> _columns;
        public InsertColumnsToken(int startIndex, List<string> columns) : base(startIndex)
        {
            _columns = columns;
        }


        public override string ToString()
        {
            return _columns.IsEmpty() ? "" : string.Join(",", _columns);
        }
    }
}
