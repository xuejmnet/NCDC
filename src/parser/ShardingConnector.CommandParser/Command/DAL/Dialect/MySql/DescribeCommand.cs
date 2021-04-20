using ShardingConnector.CommandParser.Segment.Generic.Table;

namespace ShardingConnector.CommandParser.Command.DAL.Dialect.MySql
{
/*
* @Author: xjm
* @Description:
* @Date: Monday, 12 April 2021 21:57:08
* @Email: 326308290@qq.com
*/
    public sealed class DescribeCommand:DALCommand
    {
        
        private SimpleTableSegment table;

        public SimpleTableSegment GetTable()
        {
            return table;
        }

        public void SetTable(SimpleTableSegment table)
        {
            this.table = table;
        }
    }
}