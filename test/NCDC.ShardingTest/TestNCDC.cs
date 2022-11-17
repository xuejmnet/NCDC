// using System.Data.Common;
// using MySqlConnector;
// using Xunit;
//
// namespace NCDC.ShardingTest;
//
// public class TestNCDC
// {
//     public TestNCDC()
//     {
//     }
//
//     private async Task<DbConnection> CreateMySqlConnection()
//     {
//       var dbConnection=  new MySqlConnection("server=127.0.0.1;port=3307;database=ncdctest;userid=xjm;password=abc;");
//       await dbConnection.OpenAsync();
//       return dbConnection;
//     }
//     [Fact]
//     public async Task Test1()
//     {
//          using (var dbconnection =await CreateMySqlConnection())
//          {
//              var command = dbconnection.CreateCommand();
//              command.CommandText="select * from  sysusermod"
//          }
//         var dbCommand = _dbConnection.CreateCommand();
//         dbCommand.
//     }
// }