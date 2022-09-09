using OpenConnector.Base;
using OpenConnector.CommandParser.Command.DML;
using OpenConnector.CommandParserBinder.Command.DML;
using OpenConnector.Sharding.Rewrites.Sql.Token.SimpleObject;
using OpenConnector.Sharding.Rewrites.Token.SimpleObject;

namespace OpenConnector.Sharding.Rewrites.Token.Generator.Impl.KeyGen
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