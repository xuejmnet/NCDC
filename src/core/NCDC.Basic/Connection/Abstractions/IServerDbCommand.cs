using System.Data.Common;

namespace NCDC.Basic.Connection.Abstractions;

public interface IServerDbCommand:IDisposable
{
    DbCommand GetDbCommand();
    IServerDbDataReader ExecuteReader();
    IServerDbConnection GetServerDbConnection();
}