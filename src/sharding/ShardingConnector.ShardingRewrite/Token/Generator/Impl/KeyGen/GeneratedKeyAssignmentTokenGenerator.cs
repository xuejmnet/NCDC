using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using ShardingConnector.Base;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.CommandParser.Command.DML;
using ShardingConnector.Extensions;
using ShardingConnector.CommandParserBinder.Command;
using ShardingConnector.CommandParserBinder.Command.DML;
using ShardingConnector.RewriteEngine.Sql.Token.Generator.Aware;
using ShardingConnector.RewriteEngine.Sql.Token.SimpleObject;
using ShardingConnector.ShardingAdoNet;
using ShardingConnector.ShardingRewrite.Token.SimpleObject;

namespace ShardingConnector.ShardingRewrite.Token.Generator.Impl.KeyGen
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 27 April 2021 20:50:42
* @Email: 326308290@qq.com
*/
    public sealed class GeneratedKeyAssignmentTokenGenerator:BaseGeneratedKeyTokenGenerator,IParametersAware
    {
        private ParameterContext _parameterContext;
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

        public void SetParameterContext(ParameterContext parameterContext)
        {
            this._parameterContext = parameterContext;
        }
    }
}