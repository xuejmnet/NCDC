using NCDC.Common.Config.Properties;
using NCDC.Common.Rule;
using OpenConnector.Merge.Engine;
using OpenConnector.Spi.DataBase.DataBaseType;
using System.Collections.Generic;
using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Command;
using NCDC.Basic.Parser.Command;
using NCDC.Basic.Parser.MetaData.Schema;
using OpenConnector.Merge.Engine.Decorator;
using OpenConnector.Merge.Engine.Merger;
using OpenConnector.Merge.Reader.Transparent;
using OpenConnector.StreamDataReaders;

namespace OpenConnector.Merge
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: Sunday, 18 April 2021 19:45:00
    * @Email: 326308290@qq.com
    */
    public sealed class MergeEntry
    {
        private readonly IDatabaseType _databaseType;

        private readonly SchemaMetaData _schemaMetaData;

        private readonly ConfigurationProperties _properties;

        private readonly IDictionary<IBaseRule, IResultProcessEngine> _engines = new Dictionary<IBaseRule, IResultProcessEngine>();

        public MergeEntry(IDatabaseType databaseType, SchemaMetaData schemaMetaData, ConfigurationProperties properties)
        {
            this._databaseType = databaseType;
            this._schemaMetaData = schemaMetaData;
            this._properties = properties;
        }

        /**
         * Register result process engine.
         *
         * @param rule rule
         * @param processEngine result process engine
         */
        public void RegisterProcessEngine(IBaseRule rule, IResultProcessEngine processEngine)
        {
            _engines.Add(rule, processEngine);
        }

        /**
         * Process query results.
         *
         * @param queryResults query results
         * @param sqlStatementContext SQL statement context
         * @return merged result
         * @throws SQLException SQL exception
         */
        public IStreamDataReader Process(List<IStreamDataReader> streamDataReaders, ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            var mergedResult = Merge(streamDataReaders, sqlCommandContext);
            IStreamDataReader result = null;
            if (mergedResult != null)
            {
                 result = Decorate(mergedResult, sqlCommandContext);
            }
            else
            {
                result = Decorate(streamDataReaders[0], sqlCommandContext);
            }
            return result??new TransparentMergedDataReader(streamDataReaders[0]);
        }

        private IStreamDataReader Merge(List<IStreamDataReader> streamDataReaders, ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            foreach (var engineEntry in _engines)
            {
                
                if (engineEntry.Value is IResultMergerEngine resultMergerEngine)
                {
                    var resultMerger = resultMergerEngine.NewInstance(_databaseType, engineEntry.Key, _properties, sqlCommandContext);
                    return resultMerger.Merge(streamDataReaders, sqlCommandContext, _schemaMetaData);
                }
            }
            return null;
        }

        private IStreamDataReader Decorate(IStreamDataReader streamDataReader, ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            IStreamDataReader result = null;
            foreach (var engineEntry in _engines)
            {
                if (engineEntry.Value is IResultDecoratorEngine<IBaseRule> resultDecoratorEngine)
                {
                    var resultDecorator = resultDecoratorEngine.NewInstance(_databaseType, _schemaMetaData, engineEntry.Key, _properties, sqlCommandContext);
                    result = null == result ? resultDecorator.Decorate(streamDataReader, sqlCommandContext, _schemaMetaData) : resultDecorator.Decorate(result, sqlCommandContext, _schemaMetaData);
                }
            }
            return result ?? streamDataReader;
        }
        

    }
}