using NCDC.Enums;
using NCDC.Exceptions;
using NCDC.Extensions;
using NCDC.Helpers;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Binaries;
using NCDC.ProxyServer.Connection.Abstractions;
using NCDC.ProxyServer.Executors;
using NCDC.ProxyServer.StreamMerges;

namespace NCDC.ProxyServer.ServerDataReaders;

public abstract class AbstractExecuteServerDataReader:IServerDataReader
{
    protected ShardingExecutionContext ShardingExecutionContext { get; }
    protected IQueryContext QueryContext { get; }
    protected IConnectionSession ConnectionSession => QueryContext.ConnectionSession;

    public AbstractExecuteServerDataReader(ShardingExecutionContext shardingExecutionContext,IQueryContext queryContext)
    {
        ShardingExecutionContext = shardingExecutionContext;
        QueryContext = queryContext;
    }

    public async Task<IServerResult> ExecuteDbDataReaderAsync(
        CancellationToken cancellationToken = new CancellationToken())
    {
        var executor = ServerExecuteResultExecutor.Instance;
        var executeResult =await ExecuteAsync<IExecuteResult>(executor, cancellationToken);
        return Merge(executeResult);
    }

    public abstract bool Read();

    public abstract BinaryRow GetRowData();

    protected abstract IServerResult Merge(IExecuteResult executeResult);
    protected abstract ValueTask<List<IServerDbConnection>> GetServerDbConnectionsAsync(ConnectionModeEnum connectionMode,
        string dataSourceName, int connectionSize);

    private async Task<TResult> ExecuteAsync<TResult>(IExecutor<TResult> executor,CancellationToken cancellationToken = new CancellationToken())
    {
        var resultGroups =await ExecuteAsync0<TResult>(executor, cancellationToken);
        var results =(await TaskHelper.WhenAllFastFail(resultGroups)).SelectMany(o => o)
            .ToList();
        if (results.IsEmpty())
            throw new ShardingException("sharding execute result empty");
        return executor.GetShardingMerger().StreamMerge(ConnectionSession,ShardingExecutionContext,results);
    }
    protected async Task<Task<List<TResult>>[]> ExecuteAsync0<TResult>(IExecutor<TResult> executor,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var dataSourceSqlExecutorUnits = await Task.WhenAll(ShardingExecutionContext.GetExecutionUnits()
            .GroupBy(o => o.GetDataSourceName()).Select(GetSqlExecutorGroupsAsync).ToArray());

        var waitTaskQueue = 
            dataSourceSqlExecutorUnits
            .Select(dataSourceSqlExecutorUnit =>
            {
                return  Task.Run(async () =>
                {
                    return await executor.ExecuteAsync(dataSourceSqlExecutorUnit,
                        cancellationToken);
                }, cancellationToken);
            }).ToArray();
        return waitTaskQueue;
    }
    
    /// <summary>
    /// 将各个数据源的数据进行分组后每组都有对应的执行单元
    /// 假如当前执行单元为x=[1,2,3,4,5,6,7,8,9]
    /// 我会首先判断当前是否是ExecuteNoQuery操作如果是serial=true
    /// 那么说明当前的所有命令需要串行执行
    /// 否则说明当前的执行命令可以进行并行,具体的并行度由_maxQueryConnectionsLimit参数值决定
    /// 如果_maxQueryConnectionsLimit=2,那么x将会被分解为y=[[1,2],[3,4],[5,6],[7,8],[9]]
    /// 我们将y的每一项称为执行组,同一个执行组里面的所有执行单元都将以并行方式执行,组与组之间将以串行方式执行
    /// 比如:循环y，第一次执行[1,2]，其中1命令和2命令是并行执行，执行完成后将执行[3,4]依次类推
    /// </summary>
    /// <param name="sqlGroups"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    private async Task<DataSourceSqlExecutorUnit> GetSqlExecutorGroupsAsync(IGrouping<string, ExecutionUnit> sqlGroups)
    {
        var isSerialExecute = ShardingExecutionContext.IsSerialExecute;
        var maxQueryConnectionsLimit = ShardingExecutionContext.MaxQueryConnectionsLimit;
        var dataSourceName = sqlGroups.Key;
        var sqlCount = sqlGroups.Count();


        //串行执行insert update delete或者最大连接数大于每个数据源分库的执行数目
        var connectionMode = (isSerialExecute || maxQueryConnectionsLimit >= sqlCount)
            ? ConnectionModeEnum.MEMORY_STRICTLY
            : ConnectionModeEnum.CONNECTION_STRICTLY;

        //如果是串行执行就是说每个组只有1个,如果是不是并行每个组有最大执行个数个
        var parallelCount = isSerialExecute ? 1 : maxQueryConnectionsLimit;
        
        var sqlUnitPartitions = sqlGroups.Partition(parallelCount).ToArray();
        //由于分组后除了最后一个元素其余元素都满足parallelCount为最大,第一个元素的分组数将是实际的创建连接数
        var createDbConnectionCount = sqlUnitPartitions[0].Count;

         var dbConnections =await GetServerDbConnectionsAsync(connectionMode, dataSourceName, createDbConnectionCount);
        //将SqlExecutorUnit进行分区,每个区maxQueryConnectionsLimit个
        //[1,2,3,4,5,6,7],maxQueryConnectionsLimit=3,结果就是[[1,2,3],[4,5,6],[7]]
        var sqlExecutorUnitPartitions = sqlUnitPartitions
            .Select(executionUnits =>
            {
                var commandExecuteUnits = executionUnits
                    .Select((executionUnit, i) =>
                    {
                        var sqlUnit = executionUnit.GetSqlUnit();
                        var serverDbCommand = dbConnections[i].CreateCommand(sqlUnit.GetSql(),sqlUnit.GetParameterContext().GetDbParameters());
                        return new CommandExecuteUnit(executionUnit,serverDbCommand , connectionMode);
                    })
                    .ToList();
                return commandExecuteUnits;
            });

        var sqlExecutorGroups = sqlExecutorUnitPartitions
            .Select(o => new SqlExecutorGroup<CommandExecuteUnit>(connectionMode, o)).ToList();
        return new DataSourceSqlExecutorUnit(connectionMode, sqlExecutorGroups);
    }

    public abstract void Dispose();
}