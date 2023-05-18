using System;
using System.Security.Claims;

namespace Project.Api.Helpers
{
    public static class TokenHelpers
    {
        public static int GetUserId(ClaimsPrincipal user)
        {
            var identity = (ClaimsIdentity)user.Identity;

            return Convert.ToInt32(identity.Name);
        }
    }
}
