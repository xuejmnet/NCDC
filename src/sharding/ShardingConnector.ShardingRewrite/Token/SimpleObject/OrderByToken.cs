using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShardingConnector.CommandParser.Constant;
using ShardingConnector.RewriteEngine.Sql.Token.SimpleObject;

namespace ShardingConnector.ShardingRewrite.Token.SimpleObject
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
        private readonly ICollection<string> columnLabels = new LinkedList<string>();

        private readonly ICollection<OrderDirectionEnum> orderDirections = new LinkedList<OrderDirectionEnum>();
        public OrderByToken(int startIndex) : base(startIndex)
        {
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append(" ORDER BY ");
            for (int i = 0; i < columnLabels.Count; i++)
            {
                if (0 == i)
                {
                    result.Append(columnLabels.ElementAt(i)).Append(" ").Append(orderDirections.ElementAt(i).ToString());
                }
                else
                {
                    result.Append(",").Append(columnLabels.ElementAt(i)).Append(" ").Append(orderDirections.ElementAt(i).ToString());
                }
            }
            result.Append(" ");
            return result.ToString();
        }
    }
}
