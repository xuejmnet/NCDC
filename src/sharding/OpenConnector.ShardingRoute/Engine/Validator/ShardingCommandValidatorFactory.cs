using System;
using System.Collections.Generic;
using System.Text;
using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Command;
using NCDC.CommandParser.Command.DML;
using OpenConnector.ShardingRoute.Engine.Validator.Impl;

namespace OpenConnector.ShardingRoute.Engine.Validator
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/28 11:26:47
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public static class ShardingCommandValidatorFactory
    {
        private static readonly IShardingCommandValidator ShardingInsertCommandValidator = new ShardingInsertCommandValidator();
        private static readonly IShardingCommandValidator ShardingUpdateCommandValidator = new ShardingUpdateCommandValidator();
        public static IShardingCommandValidator NewInstance(ISqlCommand sqlCommand)
        {
            if (sqlCommand is InsertCommand) {
                return ShardingInsertCommandValidator;
            }
            if (sqlCommand is UpdateCommand) {
                return ShardingUpdateCommandValidator;
            }
            return null;
        }
    }
}
