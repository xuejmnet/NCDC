using System;
using System.Collections.Generic;
using System.Linq;
using ShardingConnector.Base;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.CommandParser.Command.DML;
using ShardingConnector.Extensions;
using ShardingConnector.ParserBinder.Command;
using ShardingConnector.ParserBinder.Command.DML;
using ShardingConnector.RewriteEngine.Sql.Token.Generator.Aware;
using ShardingConnector.RewriteEngine.Sql.Token.SimpleObject;
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
        private List<object> _parameters;
        public override SqlToken GenerateSqlToken(InsertCommandContext sqlCommandContext)
        {
            var generatedKey = sqlCommandContext.GetGeneratedKeyContext();
            ShardingAssert.CantBeNull(generatedKey,"generatedKey is required");
            ShardingAssert.CantBeNull(sqlCommandContext.GetSqlCommand().SetAssignment,"setAssignment is required");
            int startIndex = sqlCommandContext.GetSqlCommand().SetAssignment.GetStopIndex() + 1;
            if (_parameters.IsEmpty())
                return new LiteralGeneratedKeyAssignmentToken(startIndex, generatedKey.GetColumnName(), generatedKey.GetGeneratedValues().LastOrDefault());
            return  new ParameterMarkerGeneratedKeyAssignmentToken(startIndex, generatedKey.GetColumnName());
        }

        public override bool IsGenerateSqlToken(InsertCommand insertCommand)
        {
            return insertCommand.SetAssignment!=null;
        }

        public void SetParameters(ICollection<object> parameters)
        {
            this._parameters = parameters.ToList();
        }
    }
}