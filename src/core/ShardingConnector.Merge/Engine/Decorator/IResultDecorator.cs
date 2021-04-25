using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.Executor;
using ShardingConnector.Merge.Reader;
using ShardingConnector.ParserBinder.Command;
using ShardingConnector.ParserBinder.MetaData.Schema;

namespace ShardingConnector.Merge.Engine.Decorator
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/25 10:41:25
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public interface IResultDecorator
    {    /**
     * Decorate query result.
     *
     * @param queryResult query result
     * @param sqlStatementContext SQL statement context
     * @param schemaMetaData schema meta data
     * @return merged result
     * @throws SQLException SQL exception
     */
        IMergedEnumerator Decorate(IQueryEnumerator queryResult, ISqlCommandContext<ISqlCommand> sqlCommandContext, SchemaMetaData schemaMetaData);

        /**
         * Decorate merged result.
         * 
         * @param mergedResult merged result
         * @param sqlStatementContext SQL statement context
         * @param schemaMetaData schema meta data
         * @return merged result
         * @throws SQLException SQL exception
         */
        IMergedEnumerator Decorate(IMergedEnumerator mergedResult, ISqlCommandContext<ISqlCommand> sqlCommandContext, SchemaMetaData schemaMetaData);

    }
}
