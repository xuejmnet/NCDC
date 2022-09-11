using OpenConnector.Base;
using NCDC.CommandParser.Command.DML;
using NCDC.ShardingParser.Command.DML;
using NCDC.Sharding.Rewrites.Sql.Token.SimpleObject;
using NCDC.Sharding.Rewrites.Sql.Token.SimpleObject.Generic;

namespace NCDC.ShardingRewrite.Token.Generator.Impl.KeyGen
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 27 April 2021 20:59:20
* @Email: 326308290@qq.com
*/
    public sealed class GeneratedKeyForUseDefaultInsertColumnsTokenGenerator:BaseGeneratedKeyTokenGenerator
    {
        public override SqlToken GenerateSqlToken(InsertCommandContext sqlCommandContext)
        {
            var insertColumnsSegment = sqlCommandContext.GetSqlCommand().InsertColumns;
            ShardingAssert.ShouldBeNotNull(insertColumnsSegment,"insertColumnsSegment is required");
            return new UseDefaultInsertColumnsToken(insertColumnsSegment.GetStopIndex(), GetColumnNames(sqlCommandContext));
        }
    
        private List<String> GetColumnNames(InsertCommandContext insertCommandContext) {
            var generatedKey = insertCommandContext.GetGeneratedKeyContext();
            ShardingAssert.ShouldBeNotNull(generatedKey,"generatedKey is required");
            List<String> result = new List<string>(insertCommandContext.GetColumnNames());
            result.Remove(generatedKey.GetColumnName());
            result.Add(generatedKey.GetColumnName());
            return result;
        }

        public override bool IsGenerateSqlToken(InsertCommand insertCommand)
        {
            return insertCommand.UseDefaultColumns();
        }
    }
}