using MySqlConnector;
using ShardingConnector.Extensions;
using ShardingConnector.ProxyServer.Abstractions;
using ShardingConnector.ProxyServer.Binaries;

using ShardingConnector.ProxyServer.Session;
using ShardingConnector.ShardingMerge.DAL.Common;
using ShardingConnector.StreamDataReaders;

namespace ShardingConnector.ProxyServer.ServerHandlers;

public sealed class ShowDatabasesServerHandler:IServerHandler
{
    private readonly ConnectionSession _connectionSession;
    private IStreamDataReader _streamDataReader;

    public ShowDatabasesServerHandler(ConnectionSession connectionSession)
    {
        _connectionSession = connectionSession;
    }
    public IServerResult Execute()
    {
        var databaseNames = GetDatabaseNames();
        _streamDataReader = new SingleLocalDataMergedDataReader(databaseNames);

        throw new NotImplementedException();
    }

    private ICollection<string> GetDatabaseNames()
    {
        var allDatabaseNames = ProxyRuntimeContext.Instance.GetAllDatabaseNames();
        var shardingConnectorUser = ProxyRuntimeContext.Instance.GetUser(_connectionSession.GetGrantee().Username);
        if (shardingConnectorUser!=null&&shardingConnectorUser.AuthorizeDatabases.IsNotEmpty())
        {
            return allDatabaseNames.Intersect(shardingConnectorUser.AuthorizeDatabases.Keys).ToList();
        }

        return allDatabaseNames;
    }

    bool IServerHandler.Read()
    {
        return _streamDataReader!.Read();
    }

    public BinaryRow GetRowData()
    {
        return new BinaryRow(new List<BinaryCell>() { new BinaryCell(_streamDataReader.GetValue(0), typeof(string)) });
    }
}