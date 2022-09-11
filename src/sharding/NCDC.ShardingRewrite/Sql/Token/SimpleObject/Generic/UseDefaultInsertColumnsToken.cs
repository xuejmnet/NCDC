using OpenConnector.Extensions;

namespace NCDC.ShardingRewrite.Sql.Token.SimpleObject.Generic
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/24 13:51:16
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class UseDefaultInsertColumnsToken:SqlToken, IAttachable
    {
        public  List<string> Columns  { get; }

        public UseDefaultInsertColumnsToken(int startIndex,List<string> columns) : base(startIndex)
        {
            this.Columns = columns;
        }

        public override string ToString()
        {
            return Columns.IsEmpty() ? string.Empty : $"({string.Join(",", Columns)})";
        }
    }
}
