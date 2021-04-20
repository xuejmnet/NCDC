﻿using System.Collections.Generic;
using ShardingConnector.Api.Database.DatabaseType;
using ShardingConnector.Common.Config.Properties;
using ShardingConnector.Common.MetaData;
using ShardingConnector.Common.Rule;
using ShardingConnector.Executor.Engine;
using ShardingConnector.Kernels.Parse;
using ShardingConnector.Spi.DataBase.DataBaseType;

namespace ShardingConnector.AdoNet.AdoNet.Core.Context
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/16 09:48:58
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public abstract class AbstractRuntimeContext<T>: ShardingConnector.AdoNet.AdoNet.Core.Context.IRuntimeContext<T> where T:IBaseRule
    {

        private readonly T rule;
    
        private readonly ConfigurationProperties properties;
    
        private readonly IDatabaseType databaseType;
    
        private readonly ExecutorEngine executorEngine;
    
        private readonly SqlParserEngine sqlParserEngine;
    
        protected AbstractRuntimeContext(T rule, IDictionary<string,object> props, IDatabaseType databaseType) {
            this.rule = rule;
            // properties = new ConfigurationProperties(null == props ? new Properties() : props);
            properties = new ConfigurationProperties();
            this.databaseType = databaseType;
            executorEngine = new ExecutorEngine();
            sqlParserEngine = SqlParserEngineFactory.GetSQLParserEngine(DatabaseTypes.GetTrunkDatabaseTypeName(databaseType));
            // ConfigurationLogger.log(rule.getRuleConfiguration());
            // ConfigurationLogger.log(props);
        }
    
        public abstract ShardingConnectorMetaData GetMetaData();
    
        public T GetRule()
        {
            return rule;
        }

        public ConfigurationProperties GetProperties()
        {
            return properties;
        }

        public IDatabaseType GetDatabaseType()
        {
            return databaseType;
        }

        public ExecutorEngine GetExecutorEngine()
        {
            return executorEngine;
        }

        public SqlParserEngine GetSqlParserEngine()
        {
            return sqlParserEngine;
        }
        public void Dispose()
        {
        }
    }
}