using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Project.Core.Entities;
using Project.Core.Enum;
using Project.Core.Exceptions;
using Project.Core.Models.Auth;
using Project.Core.OperationInterfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Project.Core.Helpers
{

    public static class AuthenticationValidation
    {
        public static async Task<ClaimsIdentity> GetIdentityByLoginPair(
            HttpContext context,
            string userName,
            string password)
        {
            try
            {
                var claims = new List<Claim>();
                var userService = context.RequestServices.GetService<IUserService>();

                var user = userService.GetByUserNameAuth(userName);
                if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Passwd))
                {
                    throw new LogicException(ExceptionMessage.INVALID_CREDENTIALS);
                }
                claims.Add(new Claim("user", JsonConvert.SerializeObject(user, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), typeof(User).FullName));
                //claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role));
                var claimIdentity = new ClaimsIdentity(new GenericIdentity(user.Id.ToString()), claims);
                return await Task.FromResult(claimIdentity);
            }
            catch (Exception ex)
            {
                return await Task.FromResult<ClaimsIdentity>(null);
            }
        }

        public static async Task<ClaimsIdentity> GetIdentityByApiKey(
            HttpContext context,
            string apiKey)
        {
            try
            {
                var claims = new List<Claim>();

                var config = context.RequestServices.GetService<TokenAuthenticationConfiguration>();

                if (apiKey != config.ApiKey)
                {
                    throw new LogicException(ExceptionMessage.INVALID_CREDENTIALS);
                }

                var claimIdentity = new ClaimsIdentity(new GenericIdentity("-1"), claims);
                return await Task.FromResult(claimIdentity);

            }
            catch (Exception ex)
            {
                return await Task.FromResult<ClaimsIdentity>(null);
            }
        }
    }
}
