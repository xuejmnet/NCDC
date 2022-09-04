using OpenConnector.Configuration;
using OpenConnector.Exceptions;
using OpenConnector.Extensions;
using OpenConnector.Helpers;
using OpenConnector.ProxyServer.Abstractions;
using OpenConnector.ProxyServer.Binaries;
using OpenConnector.ProxyServer.Commons;
using OpenConnector.ProxyServer.ServerHandlers.Results;
using OpenConnector.ProxyServer.Session.Connection.Abstractions;
using OpenConnector.ProxyServer.StreamMerges;
using OpenConnector.ProxyServer.StreamMerges.Executors.Context;
using OpenConnector.ProxyServer.StreamMerges.Results;
using CommandExecuteUnit = OpenConnector.ProxyServer.StreamMerges.Executors.Context.CommandExecuteUnit;

namespace OpenConnector.ProxyServer.ServerDataReaders;

public abstract class AbstractExecuteServerDataReader:IServerDataReader
{
    protected ShardingExecutionContext ShardingExecutionContext { get; }
    public AbstractExecuteServerDataReader(ShardingExecutionContext shardingExecutionContext)
    {
        ShardingExecutionContext = shardingExecutionContext;
    }

    public  IServerResult ExecuteDbDataReader(
        CancellationToken cancellationToken = new CancellationToken())
    {
        var executor = ServerExecuteResultExecutor.Instance;
        var executeResult = ExecuteAsync<IExecuteResult>(executor, cancellationToken).GetAwaiter().GetResult();
        return Merge(executeResult);
    }

    public abstract bool Read();

    public abstract BinaryRow GetRowData();

    protected abstract IServerResult Merge(IExecuteResult executeResult);
    protected abstract List<IServerDbConnection> GetServerDbConnections(ConnectionModeEnum connectionMode,
        string dataSourceName, int connectionSize);

    private async Task<TResult> ExecuteAsync<TResult>(IExecutor<TResult> executor,CancellationToken cancellationToken = new CancellationToken())
    {
        var resultGroups = ExecuteAsync0<TResult>(executor, cancellationToken);
        var results =(await TaskHelper.WhenAllFastFail(resultGroups)).SelectMany(o => o)
            .ToList();
        if (results.IsEmpty())
            throw new ShardingException("sharding execute result empty");
        return executor.GetShardingMerger().StreamMerge(ShardingExecutionContext,results);
    }
    protected Task<List<TResult>>[] ExecuteAsync0<TResult>(IExecutor<TResult> executor,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var waitTaskQueue = ShardingExecutionContext.GetExecutionUnits()
            .GroupBy(o => o.GetDataSourceName())
            .Select(o => GetSqlExecutorGroups(o))
            .Select(dataSourceSqlExecutorUnit =>
            {
                return Task.Run(async () =>
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
    private DataSourceSqlExecutorUnit GetSqlExecutorGroups(IGrouping<string, ExecutionUnit> sqlGroups)
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

         var dbConnections = GetServerDbConnections(connectionMode, dataSourceName, createDbConnectionCount);
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