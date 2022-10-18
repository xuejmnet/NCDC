using NCDC.Base;
using NCDC.CommandParser.Common.Command.DML;
using NCDC.CommandParser.Dialect.Handler.DML;
using NCDC.ShardingParser.Command.DML;
using NCDC.ShardingAdoNet;
using NCDC.ShardingRewrite.Sql.Token.SimpleObject;
using NCDC.ShardingRewrite.Token.SimpleObject;

namespace NCDC.ShardingRewrite.Token.Generator.Impl.KeyGen
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 27 April 2021 20:50:42
* @Email: 326308290@qq.com
*/
    public sealed class GeneratedKeyAssignmentTokenGenerator:BaseGeneratedKeyTokenGenerator
    {
        private readonly ParameterContext _parameterContext;

        public GeneratedKeyAssignmentTokenGenerator(ParameterContext parameterContext)
        {
            _parameterContext = parameterContext;
        }
        public override SqlToken GenerateSqlToken(InsertCommandContext sqlCommandContext)
        {
            var generatedKey = sqlCommandContext.GetGeneratedKeyContext();
            ShardingAssert.ShouldBeNotNull(generatedKey,"generatedKey is required");
            var insertCommand = sqlCommandContext.GetSqlCommand();
            var setAssignmentSegment = InsertCommandHandler.GetSetAssignmentSegment(insertCommand);
            ShardingAssert.ShouldBeNotNull(setAssignmentSegment,"setAssignment is required");
            int startIndex = setAssignmentSegment!.StopIndex + 1;
            if (_parameterContext.IsEmpty())
                return new LiteralGeneratedKeyAssignmentToken(startIndex, generatedKey!.GetColumnName(), generatedKey.GetGeneratedValues().LastOrDefault());
            return  new ParameterMarkerGeneratedKeyAssignmentToken(startIndex, generatedKey.GetColumnName());
        }

        public override bool IsGenerateSqlToken(InsertCommandContext insertCommandContext)
        {
            return InsertCommandHandler.GetSetAssignmentSegment(insertCommandContext.GetSqlCommand()) is not null;
        }
    }
}