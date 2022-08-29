using MySqlConnector;
using OpenConnector.Extensions;
using OpenConnector.ProxyServer.Abstractions;
using OpenConnector.ProxyServer.Binaries;

using OpenConnector.ProxyServer.Session;
using OpenConnector.ShardingMerge.DAL.Common;
using OpenConnector.StreamDataReaders;

namespace OpenConnector.ProxyServer.ServerHandlers;

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
        var OpenConnectorUser = ProxyRuntimeContext.Instance.GetUser(_connectionSession.GetGrantee().Username);
        if (OpenConnectorUser!=null&&OpenConnectorUser.AuthorizeDatabases.IsNotEmpty())
        {
            return allDatabaseNames.Intersect(OpenConnectorUser.AuthorizeDatabases.Keys).ToList();
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