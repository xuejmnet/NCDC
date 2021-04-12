using System;
using ShardingConnector.Parser.Sql.Segment.Generic;

namespace ShardingConnector.Parser.Sql.Command.DAL.Dialect.MySql
{
/*
* @Author: xjm
* @Description:
* @Date: Monday, 12 April 2021 22:17:19
* @Email: 326308290@qq.com
*/
    public sealed class ShowTableStatusCommand:DALCommand
    {
        
        private IRemoveAvailable fromSchema;
    
        /// <summary>
        /// Get from schema.
        /// </summary>
        /// <returns></returns>
        public IRemoveAvailable GetFromSchema() {
            return fromSchema;
        }

        public void SetFromSchema(IRemoveAvailable fromSchema)
        {
            this.fromSchema = fromSchema;
        }

    }
}