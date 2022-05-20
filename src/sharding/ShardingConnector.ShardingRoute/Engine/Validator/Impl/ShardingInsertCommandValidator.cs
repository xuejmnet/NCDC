using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

using ShardingConnector.CommandParser.Command;
using ShardingConnector.CommandParser.Command.DML;
using ShardingConnector.CommandParser.Segment.DML.Assignment;
using ShardingConnector.CommandParser.Segment.DML.Column;
using ShardingConnector.Exceptions;
using ShardingConnector.ShardingCommon.Core.Rule;

namespace ShardingConnector.ShardingRoute.Engine.Validator.Impl
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/29 13:10:41
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ShardingInsertCommandValidator: IShardingCommandValidator<InsertCommand>
    {
        public void Validate(ShardingRule shardingRule, InsertCommand sqlCommand, IDictionary<string, DbParameter> parameters)
        {
            var onDuplicateKeyColumnsSegment = sqlCommand.OnDuplicateKeyColumns;
            if (onDuplicateKeyColumnsSegment!=null && IsUpdateShardingKey(shardingRule, onDuplicateKeyColumnsSegment, sqlCommand.Table.GetTableName().GetIdentifier().GetValue()))
            {
                throw new ShardingException("INSERT INTO .... ON DUPLICATE KEY UPDATE can not support update for sharding column.");
            }
        }

        private bool IsUpdateShardingKey( ShardingRule shardingRule,  OnDuplicateKeyColumnsSegment onDuplicateKeyColumnsSegment,  String tableName)
        {
            foreach (var assignment in onDuplicateKeyColumnsSegment.GetColumns())
            {
                if (shardingRule.IsShardingColumn(assignment.GetColumn().GetIdentifier().GetValue(), tableName))
                {
                    return true;
                }
            }
            return false;
        }

        public void Validate(ShardingRule shardingRule, ISqlCommand sqlCommand, IDictionary<string, DbParameter> parameters)
        {
            Validate(shardingRule, (InsertCommand) sqlCommand, parameters);
        }
    }
}
