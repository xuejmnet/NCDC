using System;
using System.Linq;
using ShardingConnector.Base;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.CommandParser.Command.DML;
using ShardingConnector.CommandParser.Segment.DML.Column;
using ShardingConnector.ParserBinder.Command;
using ShardingConnector.ParserBinder.Command.DML;
using ShardingConnector.ParserBinder.Segment.Insert.Keygen;
using ShardingConnector.RewriteEngine.Sql.Token.SimpleObject;
using ShardingConnector.ShardingRewrite.Token.SimpleObject;

namespace ShardingConnector.ShardingRewrite.Token.Generator.Impl.KeyGen
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 27 April 2021 21:03:42
* @Email: 326308290@qq.com
*/
    public sealed class GeneratedKeyInsertColumnTokenGenerator:BaseGeneratedKeyTokenGenerator
    {
        public override SqlToken GenerateSqlToken(InsertCommandContext sqlCommandContext)
        {
            var generatedKey = sqlCommandContext.GetGeneratedKeyContext();
            ShardingAssert.ShouldBeNotNull(generatedKey,"generatedKey is required");
            var sqlSegment = sqlCommandContext.GetSqlCommand().InsertColumns;
            ShardingAssert.ShouldBeNotNull(sqlSegment,"sqlSegment is required");
            return new GeneratedKeyInsertColumnToken(sqlSegment.GetStopIndex(), generatedKey.GetColumnName());

        }

        public override bool IsGenerateSqlToken(InsertCommand insertCommand)
        {
            var sqlSegment = insertCommand.InsertColumns;
            return sqlSegment!=null && sqlSegment.GetColumns().Any();
        }
    }
}