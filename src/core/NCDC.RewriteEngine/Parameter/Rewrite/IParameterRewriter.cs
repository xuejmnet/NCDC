using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Command;
using NCDC.CommandParserBinder.Command;
using OpenConnector.RewriteEngine.Parameter.Builder;
using OpenConnector.ShardingAdoNet;

namespace OpenConnector.RewriteEngine.Parameter.Rewrite
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/26 15:04:09
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
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
}
