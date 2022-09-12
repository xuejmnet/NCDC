using System;
using System.Collections.Generic;
using System.Data.Common;
using OpenConnector.ShardingAdoNet;
using OpenConnector.Spi.DataBase.MetaData;

namespace OpenConnector.Executor.Hook
{
/*
* @Author: xjm
* @Description:
* @Date: Thursday, 15 April 2021 20:16:20
* @Email: 326308290@qq.com
*/
    public interface ISqlExecutionHook
    {
        
        /**
     * Handle when SQL execution started.
     * 
     * @param dataSourceName data source name
     * @param sql SQL˙
     * @param parameters parameters of SQL
     * @param dataSourceMetaData data source meta data
     * @param isTrunkThread is execution in trunk thread
     * @param shardingExecuteDataMap sharding execute data map
     */
        void Start(string dataSourceName, string sql, ParameterContext parameterContext, IDataSourceMetaData dataSourceMetaData, bool isTrunkThread, IDictionary<string, object> shardingExecuteDataMap);
    
        /**
     * Handle when SQL execution finished success.
     */
        void FinishSuccess();
    
        /**
     * Handle when SQL execution finished failure.
     *
     * @param cause failure cause
     */
        void FinishFailure(Exception exception);
    }
}