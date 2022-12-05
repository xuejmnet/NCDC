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
    private async Task<DbConnection> CreateProxyMySqlConnection1()
    {
        var dbConnection =
            new MySqlConnection("server=127.0.0.1;port=3307;database=ncdctest;userid=xjm;password=abc;");
        await dbConnection.OpenAsync();
        return dbConnection;
    }

    [Fact]
    public async Task Test1()
    {
        var column1 = new DataAssertEntity(21);
        var column2 = new DataAssertEntity(21);

        using (var dbconnection = await CreateMySqlConnection())
        {
            var command = dbconnection.CreateCommand();
            command.CommandText = "select * from  string_entity";
            var dbDataReader = await command.ExecuteReaderAsync();
            while (dbDataReader.Read())
            {
                column1.Add(dbDataReader);
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
                column2.Add(dbDataReader);
            }

            var readOnlyCollection = dbDataReader.GetColumnSchema();
        }

        Assert.Equal(column1, column2);
    }
    [Fact]
    public async Task Test2()
    {
        var column1 = new DataAssertEntity(25);
        var column2 = new DataAssertEntity(25);

        using (var dbconnection = await CreateMySqlConnection())
        {
            var command = dbconnection.CreateCommand();
            command.CommandText = "select * from  number_entity";
            var dbDataReader = await command.ExecuteReaderAsync();
            while (dbDataReader.Read())
            {
                column1.Add(dbDataReader);
            }

            var readOnlyCollection = dbDataReader.GetColumnSchema();
        }

        using (var dbconnection = await CreateProxyMySqlConnection())
        {
            var command = dbconnection.CreateCommand();
            command.CommandText = "select * from  number_entity";
            var dbDataReader = await command.ExecuteReaderAsync();
            while (dbDataReader.Read())
            {
                column2.Add(dbDataReader);
            }

            var readOnlyCollection = dbDataReader.GetColumnSchema();
        }

        Assert.Equal(column1, column2);
    }
    [Fact]
    public async Task Test3()
    {
        var column1 = new DataAssertEntity(11);
        var column2 = new DataAssertEntity(11);

        using (var dbconnection = await CreateMySqlConnection())
        {
            var command = dbconnection.CreateCommand();
            command.CommandText = "select * from  datetime_entity";
            var dbDataReader = await command.ExecuteReaderAsync();
            while (dbDataReader.Read())
            {
                column1.Add(dbDataReader);
            }

            var readOnlyCollection = dbDataReader.GetColumnSchema();
        }

        using (var dbconnection = await CreateProxyMySqlConnection())
        {
            var command = dbconnection.CreateCommand();
            command.CommandText = "select * from  datetime_entity";
            var dbDataReader = await command.ExecuteReaderAsync();
            while (dbDataReader.Read())
            {
                column2.Add(dbDataReader);
            }

            var readOnlyCollection = dbDataReader.GetColumnSchema();
        }

        Assert.Equal(column1, column2);
    }
    //mysql schema

    [Fact]
    public async Task Test4()
    {
        var proxyMySqlConnection =await CreateProxyMySqlConnection1();
        using (var dbCommand = proxyMySqlConnection.CreateCommand())
        {
            dbCommand.CommandText = "select * from sysusermodint order by age";
            using (var dataReader = await dbCommand.ExecuteReaderAsync())
            {
                var dbColumns = await dataReader.GetColumnSchemaAsync();
                var ageColumn =
                    dbColumns.FirstOrDefault(o => "age".Equals(o.ColumnName, StringComparison.OrdinalIgnoreCase));
                Assert.NotNull(ageColumn);
              
                
                int i = 1;
                while (await dataReader.ReadAsync())
                {
                    var age = dataReader.GetInt32(ageColumn.ColumnOrdinal!.Value);
                    Assert.Equal(i,age);
                    i++;
                }
            }
            
        }
        using (var dbCommand = proxyMySqlConnection.CreateCommand())
        {
            dbCommand.CommandText = "select * from sysusermodint_00";
            using (var dataReader = await dbCommand.ExecuteReaderAsync())
            {
                int i = 0;
                while (await dataReader.ReadAsync())
                {
                    i++;
                }
                Assert.Equal(333,i);
            }
        }
        using (var dbCommand = proxyMySqlConnection.CreateCommand())
        {
            dbCommand.CommandText = "select count(1) from sysusermodint_00";
            using (var dataReader = await dbCommand.ExecuteReaderAsync())
            {
                int i = 0;
                while (await dataReader.ReadAsync())
                {
                    i = dataReader.GetInt32(0);
                }
                Assert.Equal(333,i);
            }
        }
        
    }
    
}