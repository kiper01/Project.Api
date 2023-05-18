using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Project.Core.Models.Auth;
using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Project.Api.Extensions
{
    public static class AuthExtensions
    {
        public static void AddAuth(this IServiceCollection services,
            TokenAuthenticationConfiguration config,
            Func<HttpContext, string, string, Task<ClaimsIdentity>> getIdentityByLoginPairFunc,
            Func<HttpContext, string, Task<ClaimsIdentity>> getIdentityByApiKeyFunc)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config.SecretKey));

            services.AddSingleton(config);

            services.Configure<TokenProviderOptions>(options =>
            {
                options.IdentityResolver = getIdentityByLoginPairFunc;
                options.ApiKeyIdentityResolver = getIdentityByApiKeyFunc;
                options.Path = config.Path;
                options.Audience = config.Audience;
                options.Issuer = config.Issuer;
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
                options.Expiration = new TimeSpan(0, 0, 0, config.ExpirationSecondsAmount);
                options.RefreshTokenSigningKey = config.SecretKey;
            });
            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = config.Issuer,
                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = config.Audience,
                // Validate the token expiry
                ValidateLifetime = true,
                // If you want to allow a certain amount of clock drift, set that here:
                ClockSkew = TimeSpan.Zero,
            };
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(o =>
                    {
                        o.TokenValidationParameters = tokenValidationParameters;
                        o.Events = new JwtBearerEvents
                        {
                            OnMessageReceived = context =>
                            {
                                var accessToken = context.Request.Query["access_token"];
                                var path = context.HttpContext.Request.Path;
                                if (!string.IsNullOrEmpty(accessToken) &&
                                    (path.StartsWithSegments("/message-hub")))
                                {
                                    context.Token = accessToken;
                                }
                                return Task.CompletedTask;
                            }
                        };
                    });
        }
    }
}
