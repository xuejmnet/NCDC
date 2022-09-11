using System;
using System.Collections.Generic;
using System.Text;
using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Command;
using NCDC.CommandParserBinder.Command;
using NCDC.CommandParserBinder.MetaData.Schema;
using OpenConnector.StreamDataReaders;

namespace OpenConnector.Merge.Engine.Decorator
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/25 10:41:25
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public interface IDataReaderDecorator
    {
        /**
     * Decorate query result.
     *
     * @param queryResult query result
     * @param sqlStatementContext SQL statement context
     * @param schemaMetaData schema meta data
     * @return merged result
     * @throws SQLException SQL exception
     */
        IStreamDataReader Decorate(IStreamDataReader streamDataReader, ISqlCommandContext<ISqlCommand> sqlCommandContext, SchemaMetaData schemaMetaData);
    }
}