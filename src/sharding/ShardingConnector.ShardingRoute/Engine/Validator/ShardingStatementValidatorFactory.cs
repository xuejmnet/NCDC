using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.CommandParser.Command.DML;

namespace ShardingConnector.ShardingRoute.Engine.Validator
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/28 11:26:47
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ShardingStatementValidatorFactory
    {
        private ShardingStatementValidatorFactory()
        {

        }
        public static IShardingStatementValidator NewInstance(ISqlCommand sqlCommand)
        {
            if (sqlCommand is InsertCommand) {
                return new ShardingInsertStatementValidator();
            }
            if (sqlCommand is UpdateCommand) {
                return new ShardingUpdateStatementValidator();
            }
            return null;
        }
    }
}
