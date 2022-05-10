using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ShardingConnector.Base;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.Extensions;
using ShardingConnector.ParserBinder.Command;
using ShardingConnector.ParserBinder.Command.DML;
using ShardingConnector.RewriteEngine.Parameter.Builder;
using ShardingConnector.RewriteEngine.Parameter.Builder.Impl;
using ShardingConnector.RewriteEngine.Parameter.Rewrite;

namespace ShardingConnector.ShardingRewrite.Parameter.Impl
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/26 15:10:02
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ShardingGeneratedKeyInsertValueParameterRewriter:IParameterRewriter<InsertCommandContext>
    {
        public bool IsNeedRewrite(ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            return sqlCommandContext is InsertCommandContext insertCommandContext
                   && insertCommandContext.GetGeneratedKeyContext()!=null && insertCommandContext.GetGeneratedKeyContext().IsGenerated();

        }

        public void Rewrite(IParameterBuilder parameterBuilder, ISqlCommandContext<ISqlCommand> sqlCommandContext, List<object> parameters)
        {
            var insertCommandContext = (InsertCommandContext)sqlCommandContext;
            ShardingAssert.CantBeNull(insertCommandContext.GetGeneratedKeyContext(), "insertCommandContext.GetGeneratedKeyContext is required");
            ((GroupedParameterBuilder)parameterBuilder).SetDerivedColumnName(insertCommandContext.GetGeneratedKeyContext().GetColumnName());
            var generatedValues = insertCommandContext.GetGeneratedKeyContext().GetGeneratedValues().Reverse().GetEnumerator();
            int count = 0;
            int parametersCount = 0;
            foreach (var groupedParameter in insertCommandContext.GetGroupedParameters())
            {
                parametersCount += insertCommandContext.GetInsertValueContexts()[count].GetParametersCount();
                var generatedValue = generatedValues.Next();
                if (groupedParameter.Any())
                {
                    ((GroupedParameterBuilder)parameterBuilder).GetParameterBuilders()[count].AddAddedParameters(parametersCount, new List<object>(){generatedValue});
                }
                count++;
            }
        }
    }
}
