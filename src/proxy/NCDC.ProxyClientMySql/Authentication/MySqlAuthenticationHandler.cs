using System.Reflection;
using DotNetty.Transport.Channels;
using MySqlConnector;
using NCDC.Extensions;
using NCDC.Protocol.Errors;
using NCDC.Protocol.MySql.Constant;
using NCDC.Protocol.MySql.Constant.CharacterSets;
using NCDC.Protocol.MySql.Packet.Generic;
using NCDC.Protocol.MySql.Packet.Handshake;
using NCDC.Protocol.MySql.Payload;
using NCDC.Protocol.Packets;
using NCDC.ProxyClient;
using NCDC.ProxyClient.Authentication;
using NCDC.ProxyClientMySql.Authentication.Authenticator;
using NCDC.ProxyClientMySql.Common;
using NCDC.ProxyServer;
using NCDC.ProxyServer.AppServices;
using NCDC.ProxyServer.AppServices.Abstractions;
using NCDC.ProxyServer.Extensions;
using NCDC.ProxyServer.Helpers;
using NCDC.ProxyServer.Runtimes;

namespace NCDC.ProxyClientMySql.Authentication;

public sealed class MySqlAuthenticationHandler:IAuthenticationHandler<MySqlPacketPayload,MySqlAuthContext>
{ 
    private static readonly MySqlStatusFlagEnum DEFAULT_STATUS_FLAG = MySqlStatusFlagEnum.SERVER_STATUS_AUTOCOMMIT;
    private readonly IAppRuntimeManager _appRuntimeManager;

    public MySqlAuthenticationHandler(IAppRuntimeManager  appRuntimeManager)
    {
        _appRuntimeManager = appRuntimeManager;
    }
    public int Handshake(IChannelHandlerContext context, MySqlAuthContext authContext)
    { 
        int connectionId = ConnectionIdGenerator.Instance.NextId();
        authContext.Handshake();
        context.WriteAndFlushAsync(new MySqlHandshakePacket(connectionId,authContext.AuthPluginData));
        return connectionId;
    }

    public AuthenticationResult Authenticate(IChannelHandlerContext context, MySqlPacketPayload payload, MySqlAuthContext authContext)
    {
        if (MySqlConnectionPhaseEnum.AUTH_PHASE_FAST_PATH == authContext.GetStatus())
        {
            var authPhaseFastPath = AuthPhaseFastPath(context,payload,authContext);
            if (!authPhaseFastPath.Finished)
            {
                return authPhaseFastPath;
            }
        }else if (MySqlConnectionPhaseEnum.AUTHENTICATION_METHOD_MISMATCH == authContext.GetStatus())
        {
            MySqlAuthSwitchResponsePacket packet = new MySqlAuthSwitchResponsePacket(payload);
            authContext.SequenceId =packet.SequenceId;
            authContext.AuthResponse = packet.AuthPluginResponse;
        }

        var sqlErrorCode = Login(authContext);
        if (sqlErrorCode != null)
        {
            //mysql error
            context.WriteAndFlushAsync(CreateErrorPacket(sqlErrorCode, authContext));
            context.CloseAsync();
        }
        else
        {
            context.WriteAndFlushAsync(new MySqlOkPacket(++authContext.SequenceId,DEFAULT_STATUS_FLAG));
        }

        return AuthenticationResultBuilder.Finished(authContext.Username??string.Empty,authContext.HostAddress??string.Empty,authContext.Database);

        
    }
    private MySqlErrPacket CreateErrorPacket(ISqlErrorCode errorCode,MySqlAuthContext authContext) {
        
        return MySqlServerErrorCode.ER_DBACCESS_DENIED_ERROR_ARG3 == errorCode
            ? new MySqlErrPacket(++authContext.SequenceId, MySqlServerErrorCode.ER_DBACCESS_DENIED_ERROR_ARG3, authContext.Username??string.Empty, authContext.HostAddress??"unknown", authContext.Database??string.Empty)
            : new MySqlErrPacket(++authContext.SequenceId, MySqlServerErrorCode.ER_ACCESS_DENIED_ERROR_ARG3, authContext.Username??string.Empty, authContext.HostAddress??"unknown", GetErrorMessage(authContext));
    }
    private string GetErrorMessage(MySqlAuthContext authContext) {
        return 0 == authContext.AuthResponse.Length ? "NO" : "YES";
    }
    private AuthenticationResult AuthPhaseFastPath(IChannelHandlerContext context, MySqlPacketPayload payload, MySqlAuthContext authContext)
    {
        var packet = new MySqlHandshakeResponse41Packet(payload);
        authContext.AuthResponse = packet.AuthResponse;
        authContext.SequenceId = packet.SequenceId;
        authContext.Username = packet.Username;
        authContext.Database = packet.Database;
        authContext.HostAddress = RemotingHelper.GetHostAddress(context);
        var mySqlCharacterSet = MySqlCharacterSet.FromValue((MySqlCharacterSetEnum)packet.CharacterSet)??throw new NotSupportedException($"character set:[{packet.CharacterSet}]");
        context.Channel.SetEncoding(mySqlCharacterSet.Encoding);
        context.Channel.SetMySqlCharacterSet(mySqlCharacterSet);
        if (packet.Database.NotNullOrWhiteSpace() && !_appRuntimeManager.ContainsRuntimeContext(packet.Database!))
        {
            context.WriteAndFlushAsync(
                new MySqlErrPacket(++authContext.SequenceId, MySqlServerErrorCode.ER_NO_DB_ERROR));
            return AuthenticationResult.DefaultContinued;
        }
        //packet.Database
        var hostAddress =RemotingHelper.GetHostAddress(context);
        if (IsClientPluginAuth(packet) && !MySqlAuthenticationMethod.NATIVE_PASSWORD_AUTHENTICATION.Equals(packet.AuthPluginName)) {
           authContext.AuthenticationMethodMisMatchSwitch();
            context.WriteAndFlushAsync(new MySqlAuthSwitchRequestPacket(++authContext.SequenceId, MySqlAuthenticationMethod.NATIVE_PASSWORD_AUTHENTICATION,authContext.AuthPluginData));
            return AuthenticationResultBuilder.Continued(packet.Username, hostAddress, packet.Database);
        }
        return AuthenticationResultBuilder.Finished(packet.Username, hostAddress, packet.Database);

    }
    private bool IsClientPluginAuth(MySqlHandshakeResponse41Packet packet)
    {
        return packet.CapabilityFlagsEnum.HasFlag(MySqlCapabilityFlagEnum.CLIENT_PLUGIN_AUTH);
    }
    
    /// <summary>
    /// 用户登录
    /// </summary>
    /// <param name="authContext"></param>
    /// <returns>返回null说明登录成功</returns>
    private ISqlErrorCode? Login(MySqlAuthContext authContext)
    {
        if (authContext.Username is null)
        {
            throw new InvalidOperationException("unknown username");
        }

        if (!_appRuntimeManager.ContainsAppUser(authContext.Username))
        {
            return MySqlServerErrorCode.ER_ACCESS_DENIED_ERROR_ARG3;
        }
        var authUser = _appRuntimeManager.GetUser(authContext.Username);

        var mySqlAuthenticator = new MySqlNativePasswordAuthenticator(authContext.AuthPluginData);
        if (!mySqlAuthenticator.Authenticate(authUser, authContext.AuthResponse))
        {
            return MySqlServerErrorCode.ER_ACCESS_DENIED_ERROR_ARG3;
        }

        if (null == authContext.Database||!_appRuntimeManager.ContainsRuntimeContext(authContext.Database))
        {
            return MySqlServerErrorCode.ER_NO_DB_ERROR;
        }

        var authorizedUsers = _appRuntimeManager.GetAuthorizedUsers(authContext.Database);
        if (!authorizedUsers.Contains(authUser.Grantee.Username))
        {
            return MySqlServerErrorCode.ER_ACCESS_DENIED_ERROR_ARG3;
        }

        return null;

    }
}