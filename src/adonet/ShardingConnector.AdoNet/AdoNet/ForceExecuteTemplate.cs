using System;
using System.Collections.Generic;
using ShardingConnector.Exceptions;
using ShardingConnector.Extensions;

namespace ShardingConnector.AdoNet.AdoNet
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/16 14:56:33
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public sealed class ForceExecuteTemplate<T>
    {
        public void Execute(ICollection<T> targets, Action<T> callback)
        {
            ICollection<Exception> exceptions = new LinkedList<Exception>();
            foreach (var target in targets)
            {
                try
                {
                    callback(target);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            throwSQLExceptionIfNecessary(exceptions);
        }

        private void throwSQLExceptionIfNecessary(ICollection<Exception> exceptions)
        {
            if (exceptions.IsEmpty())
            {
                return;
            }
            throw new ShardingAggregateException("聚合执行出错",exceptions);
        }
    }
}