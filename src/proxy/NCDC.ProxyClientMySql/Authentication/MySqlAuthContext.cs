using NCDC.Protocol.MySql.Packet.Handshake;
using NCDC.ProxyClient.Authentication;
using NCDC.ProxyClientMySql.Common;

namespace NCDC.ProxyClientMySql.Authentication;

public sealed class MySqlAuthContext:IAuthContext
{
    private  MySqlConnectionPhaseEnum _status;
    public  string? Username { get; set; }
    public  string? Database { get; set; }
    public  string? HostAddress { get; set; }
    private  bool _isAuthenticate;
    public MySqlAuthPluginData AuthPluginData { get; }
    public byte[] AuthResponse { get; set; }
    public int SequenceId { get; set; }

    public MySqlAuthContext()
    {
        _status = MySqlConnectionPhaseEnum.INITIAL_HANDSHAKE;
        AuthPluginData=new MySqlAuthPluginData();
    }

    public bool IsAuthenticate()
    {
        return _isAuthenticate;
    }

    public void AuthenticationMethodMisMatchSwitch()
    {
        if (_status != MySqlConnectionPhaseEnum.AUTH_PHASE_FAST_PATH)
        {
            throw new InvalidOperationException($"cant {nameof(AuthenticationMethodMisMatchSwitch)},current status:{_status}");
        }

        _status = MySqlConnectionPhaseEnum.AUTHENTICATION_METHOD_MISMATCH;
    }
    public void Handshake()
    {
        if (_status != MySqlConnectionPhaseEnum.INITIAL_HANDSHAKE)
        {
            throw new InvalidOperationException($"cant {nameof(Handshake)},current status:{_status}");
        }

        _status = MySqlConnectionPhaseEnum.AUTH_PHASE_FAST_PATH;
    }

    public MySqlConnectionPhaseEnum GetStatus()
    {
        return _status;
    }
}