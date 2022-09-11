using System.Data.Common;

namespace NCDC.Basic.Connection.Abstractions;

public interface IServerDbDataReader:IDisposable
{
    DbDataReader GetDbDataReader();
    IServerDbConnection GetServerDbConnection();
}