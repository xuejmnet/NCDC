using System;
using System.Collections.Generic;
using System.Linq;
using ShardingConnector.Base;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.CommandParser.Command.DML;
using ShardingConnector.Extensions;
using ShardingConnector.ParserBinder.Command;
using ShardingConnector.ParserBinder.Command.DML;
using ShardingConnector.ParserBinder.Segment.Insert.Keygen;
using ShardingConnector.ParserBinder.Segment.Insert.Values;
using ShardingConnector.ParserBinder.Segment.Insert.Values.Expression;
using ShardingConnector.RewriteEngine.Sql.Token.Generator.Aware;
using ShardingConnector.RewriteEngine.Sql.Token.SimpleObject;
using ShardingConnector.RewriteEngine.Sql.Token.SimpleObject.Generic;

namespace ShardingConnector.ShardingRewrite.Token.Generator.Impl.KeyGen
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 27 April 2021 21:05:46
* @Email: 326308290@qq.com
*/
    public sealed class GeneratedKeyInsertValuesTokenGenerator:BaseGeneratedKeyTokenGenerator, IPreviousSqlTokensAware
    {
        private List<SqlToken> previousSqlTokens;
        public override SqlToken GenerateSqlToken(InsertCommandContext sqlCommandContext)
        {
            var result = FindPreviousSqlToken();
           ShardingAssert.CantBeNull(result,"not find PreviousSqlToken");
            var generatedKey = sqlCommandContext.GetGeneratedKeyContext();
           ShardingAssert.CantBeNull(generatedKey,"generatedKey is required");
           var generatedValues = generatedKey.GetGeneratedValues().Reverse().GetEnumerator();
            int count = 0;
            foreach (var insertValueContext in sqlCommandContext.GetInsertValueContexts())
            {
                var insertValueToken = result.InsertValues[count];
                IDerivedSimpleExpressionSegment expressionSegment;
                if (IsToAddDerivedLiteralExpression(sqlCommandContext, count))
                {
                    expressionSegment = new DerivedLiteralExpressionSegment(generatedValues.Next());
                }
                else
                {
                    expressionSegment = new DerivedParameterMarkerExpressionSegment(insertValueContext.GetParametersCount());
                }
                insertValueToken.GetValues().Add(expressionSegment);
                count++;
            }
            return result;
        }
    
        private InsertValuesToken FindPreviousSqlToken() {
            foreach (var previousSqlToken in previousSqlTokens)
            {
                if (previousSqlToken is InsertValuesToken insertValuesToken)
                    return insertValuesToken;
            }
            return null;
        }
    
        private bool IsToAddDerivedLiteralExpression(InsertCommandContext insertCommandContext, int insertValueCount) {
            return insertCommandContext.GetGroupedParameters()[insertValueCount].IsEmpty();
        }

        public override bool IsGenerateSqlToken(InsertCommand insertCommand)
        {
            return insertCommand.Values.Any();
        }

        public void SetPreviousSqlTokens(ICollection<SqlToken> previousSqlTokens)
        {
            this.previousSqlTokens = previousSqlTokens.ToList();
        }
    }
}