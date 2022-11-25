using System.Data.Common;
using MySqlConnector;
using Xunit;

namespace NCDC.ShardingTest;

public class TestNCDC
{
    public TestNCDC()
    {
    }

    private async Task<DbConnection> CreateMySqlConnection()
    {
        var dbConnection =
            new MySqlConnection("server=127.0.0.1;port=3306;database=ncdctype;userid=root;password=root;");
        await dbConnection.OpenAsync();
        return dbConnection;
    }

    private async Task<DbConnection> CreateProxyMySqlConnection()
    {
        var dbConnection =
            new MySqlConnection("server=127.0.0.1;port=3307;database=ncdctype;userid=xjm;password=abc;");
        await dbConnection.OpenAsync();
        return dbConnection;
    }

    [Fact]
    public async Task Test1()
    {
        var column1 = new DataAssertEntity(20);
        var column2 = new DataAssertEntity(20);

        using (var dbconnection = await CreateMySqlConnection())
        {
            var command = dbconnection.CreateCommand();
            command.CommandText = "select * from  string_entity";
            var dbDataReader = await command.ExecuteReaderAsync();
            while (dbDataReader.Read())
            {
                column1.Add(dbDataReader);;
            }
            var readOnlyCollection = dbDataReader.GetColumnSchema();
        }
        using (var dbconnection = await CreateProxyMySqlConnection())
        {
            var command = dbconnection.CreateCommand();
            command.CommandText = "select * from  string_entity";
            var dbDataReader = await command.ExecuteReaderAsync();
            while (dbDataReader.Read())
            {
                column2.Add(dbDataReader);;
            }

            var readOnlyCollection = dbDataReader.GetColumnSchema();
        }
       Assert.Equal(column1,column2);
    }
}