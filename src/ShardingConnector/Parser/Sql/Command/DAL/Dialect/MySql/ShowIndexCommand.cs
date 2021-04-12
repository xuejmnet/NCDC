using System;
using ShardingConnector.Parser.Sql.Segment.Generic;
using ShardingConnector.Parser.Sql.Segment.Generic.Table;

namespace ShardingConnector.Parser.Sql.Command.DAL.Dialect.MySql
{
/*
* @Author: xjm
* @Description:
* @Date: Monday, 12 April 2021 22:14:18
* @Email: 326308290@qq.com
*/
    public sealed class ShowIndexCommand:DALCommand
    {
        
        private SimpleTableSegment table;
    
        private SchemaSegment schema;
    
        /**
     * Get schema.
     * 
     * @return schema
     */
        public SchemaSegment GetSchema() {
            return schema;
        }

        public void SetSchema(SchemaSegment schema)
        {
            this.schema = schema;
        }

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