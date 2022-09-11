using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using NCDC.Basic.Connection.User;
using OpenConnector.Logger;

namespace NCDC.Basic.Metadatas;

public sealed partial class LogicDbServer:ILogicDbServer
{
    private static readonly ILogger<LogicDbServer> _logger = InternalLoggerFactory.CreateLogger<LogicDbServer>();
    private readonly IDictionary<string, ILogicDatabase> _logicDatabases = new ConcurrentDictionary<string, ILogicDatabase>();
    private readonly ConcurrentDictionary<string/*user name*/,OpenConnectorUser> _connectorUsers=new ();
    
    
    
    
    public ILogicDatabase? GetLogicDatabase(string logicDatabaseName)
    {
        if (!_logicDatabases.TryGetValue(logicDatabaseName, out var logicDatabase))
        {
            return logicDatabase;
        }

        return null;
    }

    public IEnumerable<string> GetAllLogicDatabaseNames()
    {
        return _logicDatabases.Keys;
    }


    public bool ContainsLogicDatabase(string logicDatabaseName)
    {
        throw new NotImplementedException();
    }

    public bool CreateLogicDatabase(ILogicDatabase logicDatabase)
    {
        throw new NotImplementedException();
    }

    public bool DropLogicDatabase(string logicDatabaseName)
    {
        throw new NotImplementedException();
    }
    
    public bool DatabaseExists(string? database)
    {
        if (database == null)
        {
            return false;
        }
        return _logicDatabases.ContainsKey(database);
    }

    public ILogicDatabase? GetDatabase(string? database)
    {
        if (database == null)
        {
            return null;
        }
        _logicDatabases.TryGetValue(database, out var logicDatabase);
        return logicDatabase;
    }

    public OpenConnectorUser? GetUser(string username)
    {
        _connectorUsers.TryGetValue(username, out var OpenConnectorUser);
        return OpenConnectorUser;
    }
}