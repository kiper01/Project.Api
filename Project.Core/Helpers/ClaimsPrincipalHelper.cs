using Newtonsoft.Json;
using Project.Core.Entities;
using System;
using System.Linq;
using System.Security.Claims;

namespace Project.Core.Helpers
{
    public static class ClaimsPrincipalHelper
    {
        public static User GetCurrentUser(this ClaimsPrincipal user)
        {
            var identity = (ClaimsIdentity)user.Identity;
            var currentUser = identity.Claims.FirstOrDefault(claim => claim.Type == "user")?.Value;

            return JsonConvert.DeserializeObject<User>(currentUser);
        }
        public static Guid GetCurrentUserId(this ClaimsPrincipal user)
        {
            var currentUser = GetCurrentUser(user);
            return currentUser.Id;
        }
    }
}