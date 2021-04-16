using System;
using System.Collections.Generic;
using ShardingConnector.Execute;
using ShardingConnector.Executor.Engine;

namespace ShardingConnector.Execute
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/16 08:04:37
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public sealed class SqlExecuteTemplate
    {
        public SqlExecuteTemplate(ExecutorEngine executorEngine, bool serial)
        {
            ExecutorEngine = executorEngine;
            Serial = serial;
        }

        public ExecutorEngine ExecutorEngine { get; }

        public bool Serial { get; }

        /**
     * Execute.
     *
     * @param inputGroups input groups
     * @param callback SQL execute callback
     * @param <T> class type of return value
     * @return execute result
     * @throws SQLException SQL exception
     */
        public List<R> Execute<R>(ICollection<InputGroup<CommandExecuteUnit>> inputGroups,SqlExecuteCallback<R> callback)
        {
            return Execute(inputGroups, null, callback);
        }

        /**
     * Execute.
     *
     * @param inputGroups input groups
     * @param firstCallback first SQL execute callback
     * @param callback SQL execute callback
     * @param <T> class type of return value
     * @return execute result
     * @throws SQLException SQL exception
     */
        public List<R> Execute<R>(ICollection<InputGroup<CommandExecuteUnit>> inputGroups,
            SqlExecuteCallback<R> firstCallback, SqlExecuteCallback<R> callback)
        {
            try
            {
                return ExecutorEngine.Execute(inputGroups, firstCallback, callback, Serial);
            }
            catch (Exception ex) {
                ExecutorExceptionHandler.HandleException(ex);
                return new List<R>();
            }
        }
    }
}