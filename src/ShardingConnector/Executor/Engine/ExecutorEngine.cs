using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShardingConnector.Exceptions;
using ShardingConnector.Extensions;

namespace ShardingConnector.Executor.Engine
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/14 13:00:08
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public sealed class ExecutorEngine
        /**
     * Execute.
     *
     * @param inputGroups input groups
     * @param callback grouped callback
     * @param <I> type of input value
     * @param <O> type of return value
     * @return execute result
     * @throws SQLException throw if execute failure
     */
    public List<R> Execute<T,R>(ICollection<InputGroup<T>> inputGroups, IGroupedCallback<T, R> callback){
        return Execute(inputGroups, null, callback, false);
    }
    
    /**
     * Execute.
     *
     * @param inputGroups input groups
     * @param firstCallback first grouped callback
     * @param callback other grouped callback
     * @param serial whether using multi thread execute or not
     * @param <I> type of input value
     * @param <O> type of return value
     * @return execute result
     * @throws SQLException throw if execute failure
     */
    
    public List<R> Execute<T,R>(ICollection<InputGroup<T>> inputGroups, IGroupedCallback<T, R> firstCallback, IGroupedCallback<T, R> callback, bool serial) {
        if (inputGroups.IsEmpty()) {
            return new List<R>(0);
        }
        return serial ? SerialExecute(inputGroups, firstCallback, callback) : ParallelExecute(inputGroups, firstCallback, callback);
    }
    
    private  List<R> SerialExecute<T,R>(ICollection<InputGroup<T>> inputGroups, IGroupedCallback<T, R> firstCallback, IGroupedCallback<T, R> callback) {
        ICollection<R> result = new LinkedList<R>(SyncExecute(inputGroups.First(), null == firstCallback ? callback : firstCallback));
        var loopInputGroups = inputGroups.Skip(1);
        foreach (var inputGroup in loopInputGroups)
        {
            result.AddAll(SyncExecute(inputGroup,callback));
        }
        return result.ToList();
    }
    
    private List<R> ParallelExecute<T,R>(ICollection<InputGroup<T>> inputGroups, IGroupedCallback<T, R> firstCallback, IGroupedCallback<T, R> callback) {
        var restResultFutures = AsyncExecute(inputGroups.Skip(1).ToList(), callback);
        return GetGroupResults(SyncExecute(inputGroups.First(), null == firstCallback ? callback : firstCallback), restResultFutures);
    }
    
    private ICollection<R> SyncExecute<T,R>(InputGroup<T> inputGroup, IGroupedCallback<T, R> callback){
        return callback.Execute(inputGroup.Inputs, true,new Dictionary<string, object>());
    }
    
    private  ICollection<Task<ICollection<R>>> AsyncExecute<T,R>(List<InputGroup<T>> inputGroups, IGroupedCallback<T, R> callback) {
        ICollection<Task<ICollection<R>>> result = new LinkedList<Task<ICollection<R>>>();
        foreach (var inputGroup in inputGroups)
        {
            result.Add(AsyncExecute(inputGroup,callback));
        }
        return result;
    }
    
    private  Task<ICollection<R>> AsyncExecute<T,R>(InputGroup<T> inputGroup, IGroupedCallback<T, R> callback)
    {
        IDictionary<string, object> dataMap = new Dictionary<string, object>();
        return Task.Run(()=>callback.Execute(inputGroup.Inputs, false, dataMap));
    }
    
    private  List<T> GetGroupResults<T>(ICollection<T> firstResults, ICollection<Task<ICollection<T>>> restFutures){
        ICollection<T> result = new LinkedList<T>(firstResults);
        foreach (var restFuture in restFutures)
        {
            try
            {
                result.AddAll(restFuture.GetAwaiter().GetResult());
            }
            catch (Exception e)
            {
                throw new ShardingException("get group results error",e);
            }
        }
        return result.ToList();
    }
    
    }
}