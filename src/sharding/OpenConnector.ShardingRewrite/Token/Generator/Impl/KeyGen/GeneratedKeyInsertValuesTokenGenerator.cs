using System;
using System.Collections.Generic;
using System.Linq;
using OpenConnector.Base;
using NCDC.CommandParser.Command;
using NCDC.CommandParser.Command.DML;
using OpenConnector.Extensions;
using NCDC.CommandParserBinder.Command;
using NCDC.CommandParserBinder.Command.DML;
using NCDC.CommandParserBinder.Segment.Insert.Keygen;
using NCDC.CommandParserBinder.Segment.Insert.Values;
using NCDC.CommandParserBinder.Segment.Insert.Values.Expression;
using OpenConnector.RewriteEngine.Sql.Token.Generator.Aware;
using OpenConnector.RewriteEngine.Sql.Token.SimpleObject;
using OpenConnector.RewriteEngine.Sql.Token.SimpleObject.Generic;

namespace OpenConnector.ShardingRewrite.Token.Generator.Impl.KeyGen
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
           ShardingAssert.ShouldBeNotNull(result,"not find PreviousSqlToken");
            var generatedKey = sqlCommandContext.GetGeneratedKeyContext();
           ShardingAssert.ShouldBeNotNull(generatedKey,"generatedKey is required");
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
                    expressionSegment = new DerivedParameterMarkerExpressionSegment(insertValueContext.GetParametersCount(), string.Empty);
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