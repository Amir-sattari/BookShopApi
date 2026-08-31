using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BookShopApi.Constants;

namespace BookShopApi.Helpers
{
    public static class ClaimsPrincipalExtensions
    {
        public static string? GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? user.FindFirstValue(JwtRegisteredClaimNames.NameId)
                ?? user.FindFirstValue("nameid")
                ?? user.FindFirstValue(JwtRegisteredClaimNames.Sub);
        }

        public static bool IsStaff(this ClaimsPrincipal user) =>
            user.IsInRole(AppRoles.Manager) || user.IsInRole(AppRoles.Admin);

        public static bool IsManager(this ClaimsPrincipal user) =>
            user.IsInRole(AppRoles.Manager);
    }
}
