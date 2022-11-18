using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NCDC.WebBootstrapper.Jwt;

namespace NCDC.WebBootstrapper;

public static class ApiExtension
{
    public static AuthenticationBuilder AddSecurity(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        IConfigurationSection jwtAppSettingOptions = configuration.GetSection("JwtIssuerOptions");
        services.Configure<JwtIssuerOptions>((options =>
        {
            options.Issuer = jwtAppSettingOptions["Issuer"];
            options.Audience = jwtAppSettingOptions["Audience"];
            options.SecretKey = jwtAppSettingOptions["SecretKey"];
        }));
        return services.AddAuthentication(op =>
        {
            op.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwt(jwtAppSettingOptions);
    }

    public static AuthenticationBuilder AddJwt(this AuthenticationBuilder builder,
        IConfigurationSection jwtAppSettingOptions)
    {
        
        SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtAppSettingOptions["SecretKey"]));
        builder.AddJwtBearer((option =>
        {
            option.ClaimsIssuer = jwtAppSettingOptions["Issuer"];
            option.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions["Issuer"],
                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions["Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            option.SaveToken = true;
        }));
        return builder;
    }
}