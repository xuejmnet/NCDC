using System;
using ShardingConnector.Common.Config.Properties;
using ShardingConnector.Common.Rule;
using ShardingConnector.Executor.Engine;
using ShardingConnector.Kernels.Parse;
using ShardingConnector.Spi.DataBase.DataBaseType;

namespace ShardingConnector.ShardingAdoNet.AdoNet.Core.Context
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/14 00:00:00
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    /// <summary>
    /// 
    /// </summary>
    public interface IRuntimeContext<T>:IDisposable where T:IBaseRule
    {
        
        /**
     * Get rule.
     * 
     * @return rule
     */
        T GetRule();
    
        /**
     * Get properties.
     *
     * @return properties
     */
        ConfigurationProperties GetProperties();
    
        /**
     * Get database type.
     * 
     * @return database type
     */
        IDatabaseType GetDatabaseType();
    
        /**
     * Get execute engine.
     * 
     * @return execute engine
     */
        ExecutorEngine GetExecutorEngine();
    
        /**
     * Get SQL parser engine.
     * 
     * @return SQL parser engine
     */
        SqlParserEngine GetSqlParserEngine();
    }
}