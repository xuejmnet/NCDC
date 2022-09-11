using System;
using System.Linq;
using OpenConnector.Base;
using NCDC.CommandParser.Command;
using NCDC.CommandParser.Command.DML;
using NCDC.CommandParser.Segment.DML.Column;
using NCDC.CommandParserBinder.Command;
using NCDC.CommandParserBinder.Command.DML;
using NCDC.CommandParserBinder.Segment.Insert.Keygen;
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