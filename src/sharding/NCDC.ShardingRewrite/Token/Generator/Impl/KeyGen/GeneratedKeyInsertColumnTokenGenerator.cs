using NCDC.Base;
using NCDC.CommandParser.Common.Command.DML;
using NCDC.Extensions;
using NCDC.ShardingParser.Command.DML;
using NCDC.ShardingRewrite.Sql.Token.SimpleObject;
using NCDC.ShardingRewrite.Token.SimpleObject;

namespace NCDC.ShardingRewrite.Token.Generator.Impl.KeyGen
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
            return new GeneratedKeyInsertColumnToken(sqlSegment.StopIndex, generatedKey.GetColumnName());

        }

        public override bool IsGenerateSqlToken(InsertCommandContext insertCommandContext)
        {
            var insertColumnsSegment = insertCommandContext.GetSqlCommand().InsertColumns;
            return insertColumnsSegment is not null && insertColumnsSegment.Columns.IsNotEmpty()
                                                    && insertCommandContext.GetGeneratedKeyContext() is not null
                                                    && insertCommandContext.GetGeneratedKeyContext()!
                                                        .GetGeneratedValues().IsNotEmpty();
        }
    }
}