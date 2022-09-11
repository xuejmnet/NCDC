using NCDC.CommandParser.Abstractions;
using NCDC.Basic.Parser.Command;
using NCDC.ShardingAdoNet;

namespace NCDC.Sharding.Rewrites.Abstractions;


public interface IParameterRewriter<out T> where T:ISqlCommandContext<ISqlCommand>
{
    /**
         * Judge whether need rewrite.
         *
         * @param sqlStatementContext SQL statement context
         * @return is need rewrite or not
         */
    bool IsNeedRewrite(ISqlCommandContext<ISqlCommand> sqlCommandContext);

    /**
         * Rewrite SQL parameters.
         * 
         * @param parameterBuilder parameter builder
         * @param sqlStatementContext SQL statement context
         * @param parameters SQL parameters
         */
    void Rewrite(IParameterBuilder parameterBuilder, ISqlCommandContext<ISqlCommand> sqlCommandContext, ParameterContext parameterContext);
}