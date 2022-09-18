namespace NCDC.ProxyClient.Authentication;

public interface IAuthenticatorFactory
{
    IAuthenticator Create();
}