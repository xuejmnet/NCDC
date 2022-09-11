using NCDC.CommandParser.Segment.Generic;
using NCDC.CommandParser.Segment.Generic.Table;

namespace NCDC.CommandParser.Command.DAL.Dialect.MySql
{
/*
* @Author: xjm
* @Description:
* @Date: Monday, 12 April 2021 22:01:08
* @Email: 326308290@qq.com
*/
    public sealed class ShowColumnsCommand:DALCommand
    {
        
        private SimpleTableSegment table;
    
        private IRemoveAvailable fromSchema;
    
        /// <summary>
        /// Get from schema.
        /// </summary>
        /// <returns></returns>
        public IRemoveAvailable GetFromSchema() {
            return fromSchema;
        }

        public SimpleTableSegment GetTable()
        {
            return table;
        }

        public void SetTable(SimpleTableSegment table)
        {
            this.table = table;
        }

        public void SetFromSchema(IRemoveAvailable fromSchema)
        {
            this.fromSchema = fromSchema;
        }
    }
}