using System;
using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Command;
using NCDC.CommandParser.Command.DAL.Dialect;
using NCDC.Common.Config.Properties;
using NCDC.Common.Rule;
using OpenConnector.Merge.Engine.Merger;
using OpenConnector.Merge.Engine.Merger.Impl;
using NCDC.CommandParserBinder.Command;
using NCDC.CommandParserBinder.Command.DML;
using OpenConnector.ShardingCommon.Core.Rule;
using OpenConnector.ShardingMerge.DAL;
using OpenConnector.ShardingMerge.DQL;
using OpenConnector.Spi.DataBase.DataBaseType;

namespace OpenConnector.ShardingMerge
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: Wednesday, 05 May 2021 21:16:33
    * @Email: 326308290@qq.com
    */
    public class ShardingEnumeratorMergerEngine : IResultMergerEngine<ShardingRule>
    {
        public IDataReaderMerger NewInstance(IDatabaseType databaseType, IBaseRule rule, ConfigurationProperties properties, ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            return NewInstance(databaseType, (ShardingRule) rule, properties, sqlCommandContext);
        }
        public IDataReaderMerger NewInstance(IDatabaseType databaseType, ShardingRule rule, ConfigurationProperties properties, ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            if (sqlCommandContext is SelectCommandContext)
            {
                return new ShardingDQLEnumeratorMerger(databaseType);
            }
            if (sqlCommandContext.GetSqlCommand() is DALCommand)
            {
                return new ShardingDALEnumeratorMerger(rule);
            }
            return new TransparentDataReaderMerger();
        }

        public int GetOrder()
        {
            return 0;
        }

        public Type GetGenericType()
        {
            return typeof(ShardingRule);
        }

    }
}