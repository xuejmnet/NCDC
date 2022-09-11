using System;
using System.Collections.Generic;
using System.Text;
using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Command;
using NCDC.ShardingParser.Command;
using NCDC.ShardingParser.MetaData.Schema;
using NCDC.Common.Config.Properties;
using NCDC.Common.Rule;
using OpenConnector.Spi.DataBase.DataBaseType;

namespace OpenConnector.Merge.Engine.Decorator
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/25 10:42:38
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public interface IResultDecoratorEngine<T>:IResultProcessEngine<T> where T:IBaseRule
    {
        IDataReaderDecorator NewInstance(IDatabaseType databaseType, SchemaMetaData schemaMetaData, T rule, ConfigurationProperties properties, ISqlCommandContext<ISqlCommand> sqlCommandContext);

    }
}
