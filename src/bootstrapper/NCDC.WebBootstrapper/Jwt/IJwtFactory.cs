namespace NCDC.WebBootstrapper.Jwt;

public interface IJwtFactory
{
    string CreateToken(string uid, Dictionary<string, string> cs, TimeSpan expires);

    IDictionary<string, object> Decode(string token);
}