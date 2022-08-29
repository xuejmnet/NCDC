using System;
using System.Linq;
using OpenConnector.Base;
using OpenConnector.CommandParser.Command;
using OpenConnector.CommandParser.Command.DML;
using OpenConnector.CommandParser.Segment.DML.Column;
using OpenConnector.CommandParserBinder.Command;
using OpenConnector.CommandParserBinder.Command.DML;
using OpenConnector.CommandParserBinder.Segment.Insert.Keygen;
using OpenConnector.RewriteEngine.Sql.Token.SimpleObject;
using OpenConnector.ShardingRewrite.Token.SimpleObject;

namespace OpenConnector.ShardingRewrite.Token.Generator.Impl.KeyGen
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