using OpenConnector.Base;
using NCDC.CommandParser.Command.DML;
using NCDC.CommandParserBinder.Command.DML;
using NCDC.Sharding.Rewrites.Sql.Token.SimpleObject;
using NCDC.Sharding.Rewrites.Token.SimpleObject;
using NCDC.ShardingAdoNet;

namespace NCDC.Sharding.Rewrites.Token.Generator.Impl.KeyGen
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
            ShardingAssert.ShouldBeNotNull(sqlCommandContext.GetSqlCommand().SetAssignment,"setAssignment is required");
            int startIndex = sqlCommandContext.GetSqlCommand().SetAssignment.GetStopIndex() + 1;
            if (_parameterContext.IsEmpty())
                return new LiteralGeneratedKeyAssignmentToken(startIndex, generatedKey.GetColumnName(), generatedKey.GetGeneratedValues().LastOrDefault());
            return  new ParameterMarkerGeneratedKeyAssignmentToken(startIndex, generatedKey.GetColumnName());
        }

        public override bool IsGenerateSqlToken(InsertCommand insertCommand)
        {
            return insertCommand.SetAssignment!=null;
        }
    }
}