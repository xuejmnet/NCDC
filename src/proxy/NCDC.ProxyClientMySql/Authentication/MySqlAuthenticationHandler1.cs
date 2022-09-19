// using NCDC.Protocol.Errors;
// using NCDC.Protocol.MySql.Constant;
// using NCDC.Protocol.MySql.Packet.Handshake;
// using NCDC.ProxyClientMySql.Authentication.Authenticator;
// using NCDC.ProxyServer.Contexts;
//
// namespace NCDC.ProxyClientMySql.Authentication;
//
// public class MySqlAuthenticationHandler1
// {
//     private readonly IContextManager _contextManager;
//
//     public MySqlAuthPluginData AuthPluginData = new MySqlAuthPluginData();
//
//     public MySqlAuthenticationHandler(IContextManager contextManager)
//     {
//         _contextManager = contextManager;
//     }
//
//     /// <summary>
//     /// 用户登录
//     /// </summary>
//     /// <param name="username"></param>
//     /// <param name="hostname"></param>
//     /// <param name="authResponse"></param>
//     /// <param name="databaseName"></param>
//     /// <returns>返回null说明登录成功</returns>
//     public ISqlErrorCode? Login(string username,string hostname, byte[] authResponse, string? databaseName)
//     {
//         var authUser = _contextManager.TryGetUser(username);
//
//         if (authUser == null)
//         {
//             return MySqlServerErrorCode.ER_ACCESS_DENIED_ERROR_ARG3;
//         }
//         var mySqlAuthenticator = GetAuthenticator(username,hostname);
//         if (!mySqlAuthenticator.Authenticate(authUser, authResponse))
//         {
//             return MySqlServerErrorCode.ER_ACCESS_DENIED_ERROR_ARG3;
//         }
//
//         var databaseHasPerm = true;
//         return (null==databaseName || databaseHasPerm)?null:MySqlServerErrorCode.ER_ACCESS_DENIED_ERROR_ARG3;
//         // var proxyUser = GetUser(username);
//         // if (proxyUser == null || !IsPasswordRight(proxyUser.Password, authResponse))
//         // {
//         //     return MySqlServerErrorCodeProvider.Instance().ER_DBACCESS_DENIED_ERROR;
//         // }
//         //
//         // if (!IsAuthorizedSchema(proxyUser.AuthorizedSchemas, database))
//         // {
//         //     return MySqlServerErrorCodeProvider.Instance().ER_DBACCESS_DENIED_ERROR;
//         // }
//         //
//         // return null;
//     }
//
//     // private ProxyUser? GetUser(string username)
//     // {
//     //     if (SHARDING_PROXY_CONTEXT.Authentication.Users.TryGetValue(username, out var proxyUser))
//     //     {
//     //         return proxyUser;
//     //     }
//     //
//     //     return null;
//     // }
//
//     // public bool IsPasswordRight(string password, byte[] authResponse)
//     // {
//     //     var authCipherBytes = GetAuthCipherBytes(password);
//     //     return string.IsNullOrEmpty(password) || authCipherBytes.SequenceEqual(authResponse);
//     // }
//     //
//     // private bool IsAuthorizedSchema(ICollection<string> authorizedSchemas, string? schema)
//     // {
//     //     return string.IsNullOrEmpty(schema) || authorizedSchemas.Count == 0 || authorizedSchemas.Contains(schema);
//     // }
//
//     public IMySqlAuthenticator GetAuthenticator(string username, string hostname)
//     {
//         return new MySqlNativePasswordAuthenticator(AuthPluginData);
//     }
//     
// }