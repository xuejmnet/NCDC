using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.Base;
using ShardingConnector.CommandParser.Command;
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

        public void Rewrite(IParameterBuilder parameterBuilder, InsertCommandContext insertCommandContext, List<object> parameters)
        {
            ShardingAssert.CantBeNull(insertCommandContext.GetGeneratedKeyContext(), "insertCommandContext.GetGeneratedKeyContext is required");
            ((GroupedParameterBuilder)parameterBuilder).SetDerivedColumnName(insertCommandContext.GetGeneratedKeyContext().GetColumnName());
            Iterator < Comparable <?>> generatedValues = insertStatementContext.getGeneratedKeyContext().get().getGeneratedValues().descendingIterator();
            int count = 0;
            int parametersCount = 0;
            for (List<Object> each : insertStatementContext.getGroupedParameters())
            {
                parametersCount += insertStatementContext.getInsertValueContexts().get(count).getParametersCount();
                Comparable <?> generatedValue = generatedValues.next();
                if (!each.isEmpty())
                {
                    ((GroupedParameterBuilder)parameterBuilder).getParameterBuilders().get(count).addAddedParameters(parametersCount, Lists.newArrayList(generatedValue));
                }
                count++;
            }
        }
    }
}
