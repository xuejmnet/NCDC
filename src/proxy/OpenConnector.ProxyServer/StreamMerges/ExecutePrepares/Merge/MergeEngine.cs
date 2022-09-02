﻿using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParser.Command;
using OpenConnector.CommandParserBinder.Command;
using OpenConnector.CommandParserBinder.MetaData.Schema;
using OpenConnector.Common.Config.Properties;
using OpenConnector.Common.Rule;
using OpenConnector.Exceptions;
using OpenConnector.Merge;
using OpenConnector.Merge.Engine;
using OpenConnector.Spi.DataBase.DataBaseType;
using OpenConnector.Spi.Order;
using OpenConnector.StreamDataReaders;

namespace OpenConnector.ProxyServer.StreamMerges.ExecutePrepares.Merge
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 14:47:21
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class MergeEngine
    {
        private readonly ICollection<IBaseRule> rules;

        private readonly MergeEntry merger;

        public MergeEngine(ICollection<IBaseRule> rules, ConfigurationProperties properties, IDatabaseType databaseType, SchemaMetaData metaData)
        {
            this.rules = rules;
            merger = new MergeEntry(databaseType, metaData, properties);
        }

        /**
     * Merge.
     *
     * @param queryResults query results
     * @param sqlStatementContext SQL statement context
     * @return merged result
     * @throws SQLException SQL exception
     */
        public IStreamDataReader Merge(List<IStreamDataReader> streamDataReaders, ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            RegisterMergeDecorator();
            return merger.Process(streamDataReaders, sqlCommandContext);
        }

        private void RegisterMergeDecorator()
        {
            foreach (var orderAware in OrderedRegistry.GetRegisteredOrderedAware<IResultProcessEngine>())
            {
                var processEngine = CreateProcessEngine(orderAware.GetType());
                var ruleType = orderAware.GetGenericType();
                rules.Where(o => ruleType.IsInstanceOfType(o)).ToList().ForEach(rule => merger.RegisterProcessEngine(rule, processEngine));
            }
        }

        private IResultProcessEngine CreateProcessEngine(Type type)
        {
            try
            {
                return (IResultProcessEngine)Activator.CreateInstance(type);
            }
            catch (Exception ex)
            {
                throw new ShardingException($"Can not find public default constructor for result process engine `{type}`", ex);
            }
        }
    }
}