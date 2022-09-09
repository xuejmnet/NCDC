using OpenConnector.CommandParser.Segment.DML.Expr;
using OpenConnector.Sharding.Rewrites.Sql.Token.SimpleObject.Generic;

namespace OpenConnector.Sharding.Rewrites.Token.SimpleObject
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 27 April 2021 20:25:43
* @Email: 326308290@qq.com
*/
    public sealed class ShardingInsertValue:InsertValue
    {
        private readonly ICollection<DataNode> dataNodes;
    
        public ShardingInsertValue(List<IExpressionSegment> values,ICollection<DataNode> dataNodes) : base(values)
        {
            this.dataNodes = dataNodes;
        }

        public ICollection<DataNode> GetDataNodes()
        {
            return dataNodes;
        }
    }
}