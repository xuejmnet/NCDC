using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Exceptions;
using JWT.Serializers;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace NCDC.WebBootstrapper.Jwt;

public class JwtFactory : IJwtFactory
{
    private readonly JwtIssuerOptions _jwtOptions;

    public JwtFactory(IOptions<JwtIssuerOptions> jwtOptions)
    {
        this._jwtOptions = jwtOptions.Value;
    }

    public string CreateToken(
        string uid,
        Dictionary<string, string> cs,
        TimeSpan expires)
    {
        var builder = new JwtBuilder()
            .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
            .Issuer(_jwtOptions.Issuer)
            .Audience(_jwtOptions.Audience)
            .WithSecret(_jwtOptions.SecretKey)
            .AddClaim("exp", DateTimeOffset.UtcNow.Add(expires).ToUnixTimeSeconds())
            .AddClaim("sub", Guid.NewGuid().ToString().ToLower())
            .AddClaim("uid", uid);

        if (cs.Count > 0)
        {
            foreach (KeyValuePair<string, string> c in cs)
                builder.AddClaim(c.Key, c.Value);
        }

        return builder.Encode();
    }

    public IDictionary<string, object> Decode(string token)
    {
        try
        {
            IJsonSerializer serializer = new JsonNetSerializer();
            var provider = new UtcDateTimeProvider();
            IJwtValidator validator = new JwtValidator(serializer, provider);
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm(); // symmetric
            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);

            var json = decoder.Decode(token, _jwtOptions.SecretKey, verify: true);
            Console.WriteLine(json);
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
        }
        catch (TokenExpiredException)
        {
            Console.WriteLine("Token has expired");
            throw;
        }
        catch (SignatureVerificationException)
        {
            Console.WriteLine("Token has invalid signature");
            throw;
        }
    }
}