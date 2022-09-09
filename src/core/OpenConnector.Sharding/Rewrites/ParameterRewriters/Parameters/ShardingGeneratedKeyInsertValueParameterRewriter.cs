using OpenConnector.Base;
using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParserBinder.Command;
using OpenConnector.CommandParserBinder.Command.DML;
using OpenConnector.Extensions;
using OpenConnector.Sharding.Rewrites.Abstractions;
using OpenConnector.Sharding.Rewrites.ParameterRewriters.ParameterBuilders;
using OpenConnector.ShardingAdoNet;

namespace OpenConnector.Sharding.Rewrites.ParameterRewriters.Parameters
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

        public void Rewrite(IParameterBuilder parameterBuilder, ISqlCommandContext<ISqlCommand> sqlCommandContext, ParameterContext parameterContext)
        {
            var insertCommandContext = (InsertCommandContext)sqlCommandContext;
            ShardingAssert.ShouldBeNotNull(insertCommandContext.GetGeneratedKeyContext(), "insertCommandContext.GetGeneratedKeyContext is required");
            ((GroupedParameterBuilder)parameterBuilder).SetDerivedColumnName(insertCommandContext.GetGeneratedKeyContext().GetColumnName());
            var generatedValues = insertCommandContext.GetGeneratedKeyContext().GetGeneratedValues().Reverse().GetEnumerator();
            int count = 0;
            int parametersCount = 0;
            foreach (var groupedParameter in insertCommandContext.GetGroupedParameters())
            {
                parametersCount += insertCommandContext.GetInsertValueContexts()[count].GetParametersCount();
                var generatedValue = generatedValues.Next();
                if (!groupedParameter.IsEmpty())
                {
                    ((GroupedParameterBuilder)parameterBuilder).GetParameterBuilders()[count].AddAddedParameters(parametersCount, new List<object>(){generatedValue});
                }
                count++;
            }
        }
    }
}
