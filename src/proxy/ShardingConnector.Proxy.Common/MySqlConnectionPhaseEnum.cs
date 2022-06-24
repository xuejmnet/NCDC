namespace ShardingConnector.Proxy.Common;

/// <summary>
/// <example><a href="https://dev.mysql.com/doc/internals/en/connection-phase.html">Connection Phase</a></example>
/// </summary>
public enum MySqlConnectionPhaseEnum
{
    INITIAL_HANDSHAKE, AUTH_PHASE_FAST_PATH, AUTHENTICATION_METHOD_MISMATCH
}