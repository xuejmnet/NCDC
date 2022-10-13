using System.Text;
using NCDC.CommandParser.Common.Constant;
using NCDC.ShardingRewrite.Sql.Token.SimpleObject;

namespace NCDC.ShardingRewrite.Token.SimpleObject
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/27 16:31:32
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class OrderByToken:SqlToken,IAttachable
    {
        public readonly ICollection<string> ColumnLabels = new LinkedList<string>();

        public readonly ICollection<OrderDirectionEnum> OrderDirections = new LinkedList<OrderDirectionEnum>();
        public OrderByToken(int startIndex) : base(startIndex)
        {
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append(" ORDER BY ");
            for (int i = 0; i < ColumnLabels.Count; i++)
            {
                if (0 == i)
                {
                    result.Append(ColumnLabels.ElementAt(i)).Append(" ").Append(OrderDirections.ElementAt(i).ToString());
                }
                else
                {
                    result.Append(",").Append(ColumnLabels.ElementAt(i)).Append(" ").Append(OrderDirections.ElementAt(i).ToString());
                }
            }
            result.Append(" ");
            return result.ToString();
        }
    }
}
