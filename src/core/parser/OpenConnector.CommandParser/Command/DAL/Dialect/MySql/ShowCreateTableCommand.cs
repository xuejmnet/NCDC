using OpenConnector.CommandParser.Segment.Generic.Table;

namespace OpenConnector.CommandParser.Command.DAL.Dialect.MySql
{
/*
* @Author: xjm
* @Description:
* @Date: Monday, 12 April 2021 22:12:33
* @Email: 326308290@qq.com
*/
    public sealed class ShowCreateTableCommand:DALCommand
    {
        private SimpleTableSegment table;

        public void SetTable(SimpleTableSegment table)
        {
            this.table = table;
        }

        public SimpleTableSegment GetTable()
        {
            return this.table;
        }
    }
}