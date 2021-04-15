using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed class ExecutorEngine:IDisposable
    {
    
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
        return serial ? SerialExecute(inputGroups, firstCallback, callback) : parallelExecute(inputGroups, firstCallback, callback);
    }
    
    private  List<R> SerialExecute<T,R>(ICollection<InputGroup<T>> inputGroups, IGroupedCallback<T, R> firstCallback, IGroupedCallback<T, R> callback) {
        Iterator<InputGroup<I>> inputGroupsIterator = inputGroups.iterator();
        InputGroup<I> firstInputs = inputGroupsIterator.next();
        List<O> result = new LinkedList<>(SyncExecute(firstInputs, null == firstCallback ? callback : firstCallback));
        for (InputGroup<I> each : Lists.newArrayList(inputGroupsIterator)) {
            result.addAll(syncExecute(each, callback));
        }
        return result;
    }
    
    private List<R> ParallelExecute<T,R>(ICollection<InputGroup<T>> inputGroups, IGroupedCallback<T, R> firstCallback, IGroupedCallback<T, R> callback) {
        Iterator<InputGroup<I>> inputGroupsIterator = inputGroups.iterator();
        InputGroup<I> firstInputs = inputGroupsIterator.next();
        Collection<ListenableFuture<Collection<O>>> restResultFutures = asyncExecute(Lists.newArrayList(inputGroupsIterator), callback);
        return getGroupResults(syncExecute(firstInputs, null == firstCallback ? callback : firstCallback), restResultFutures);
    }
    
    private ICollection<R> SyncExecute<T,R>(InputGroup<T> inputGroup, IGroupedCallback<T, R> callback){
        return callback.Execute(inputGroup.Inputs, true, ExecutorDataMap.getValue());
    }
    
    private <I, O> Collection<ListenableFuture<Collection<O>>> asyncExecute(final List<InputGroup<I>> inputGroups, final GroupedCallback<I, O> callback) {
        Collection<ListenableFuture<Collection<O>>> result = new LinkedList<>();
        for (InputGroup<I> each : inputGroups) {
            result.add(asyncExecute(each, callback));
        }
        return result;
    }
    
    private <I, O> ListenableFuture<Collection<O>> asyncExecute(final InputGroup<I> inputGroup, final GroupedCallback<I, O> callback) {
        final Map<String, Object> dataMap = ExecutorDataMap.getValue();
        return executorService.getExecutorService().submit(() -> callback.execute(inputGroup.getInputs(), false, dataMap));
    }
    
    private <O> List<O> getGroupResults(final Collection<O> firstResults, final Collection<ListenableFuture<Collection<O>>> restFutures) throws SQLException {
        List<O> result = new LinkedList<>(firstResults);
        for (ListenableFuture<Collection<O>> each : restFutures) {
            try {
                result.addAll(each.get());
            } catch (final InterruptedException | ExecutionException ex) {
                return throwException(ex);
            }
        }
        return result;
    }
    
    private <O> List<O> throwException(final Exception exception) throws SQLException {
        if (exception.getCause() instanceof SQLException) {
            throw (SQLException) exception.getCause();
        }
        throw new ShardingSphereException(exception);
    }
    
    @Override
    public void close() {
        executorService.close();
    }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}