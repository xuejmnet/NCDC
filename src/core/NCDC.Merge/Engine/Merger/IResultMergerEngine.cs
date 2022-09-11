using System;
using System.Collections.Generic;
using System.Text;
using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Command;
using NCDC.Basic.Parser.Command;
using NCDC.Common.Config.Properties;
using NCDC.Common.Rule;
using OpenConnector.Spi.DataBase.DataBaseType;

namespace OpenConnector.Merge.Engine.Merger
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/25 10:37:59
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public interface IResultMergerEngine : IResultProcessEngine<IBaseRule>
    {
        IDataReaderMerger NewInstance(IDatabaseType databaseType, IBaseRule rule, ConfigurationProperties properties, ISqlCommandContext<ISqlCommand> sqlCommandContext);

    }
    public interface IResultMergerEngine<in T>:IResultMergerEngine where  T:IBaseRule
    {
        IDataReaderMerger NewInstance(IDatabaseType databaseType, T rule, ConfigurationProperties properties, ISqlCommandContext<ISqlCommand> sqlCommandContext);

    }
}
