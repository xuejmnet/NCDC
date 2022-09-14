using MySqlConnector;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Binaries;
using NCDC.ProxyServer.Connection.Abstractions;
using NCDC.ShardingMerge.DataReaderMergers.DAL.Common;
using NCDC.StreamDataReaders;

namespace NCDC.ProxyServer.ServerHandlers;

public sealed class ShowDatabasesServerHandler:IServerHandler
{
    private readonly IConnectionSession _connectionSession;
    private IStreamDataReader _streamDataReader;

    public ShowDatabasesServerHandler(IConnectionSession connectionSession)
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