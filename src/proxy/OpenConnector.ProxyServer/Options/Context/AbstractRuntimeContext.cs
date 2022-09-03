using OpenConnector.Api.Database.DatabaseType;
using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParser.SqlParseEngines;
using OpenConnector.Common.Config.Properties;
using OpenConnector.Common.MetaData;
using OpenConnector.Common.Rule;
using OpenConnector.Executor.Engine;
using OpenConnector.ParserEngine;

using OpenConnector.Spi.DataBase.DataBaseType;

namespace OpenConnector.ProxyServer.Options.Context
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
    public abstract class AbstractRuntimeContext<T> : IRuntimeContext<T> where T : IBaseRule
    {
        private readonly T rule;

        private readonly ConfigurationProperties properties;

        private readonly IDatabaseType databaseType;

        // private readonly ExecutorEngine executorEngine;

        private readonly SqlCommandParser _sqlCommandParser;

        protected AbstractRuntimeContext(T rule,ISqlParserConfiguration sqlParserConfiguration, IDictionary<string, object> props, IDatabaseType databaseType)
        {
            this.rule = rule;
            properties = new ConfigurationProperties();
            this.databaseType = databaseType;
            // executorEngine = ExecutorEngine.Instance;
            //更加数据库类型获取对应的解析器
            _sqlCommandParser = new SqlCommandParser(sqlParserConfiguration);
           
        }

        /// <summary>
        /// 获取元数据信息后续用来实现刷新表数据
        /// </summary>
        /// <returns></returns>
        public abstract OpenConnectorMetaData GetMetaData();

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

        // public ExecutorEngine GetExecutorEngine()
        // {
        //     return executorEngine;
        // }

        public ISqlCommandParser GetSqlParserEngine()
        {
            return _sqlCommandParser;
        }

        public void Dispose()
        {
        }
    }
}