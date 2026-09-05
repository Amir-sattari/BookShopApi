using Microsoft.AspNetCore.Http;

namespace BookShopApi.Helpers
{
    public static class AuthCookie
    {
        public const string AccessTokenName = "bookshop_access_token";
        public static readonly TimeSpan Lifetime = TimeSpan.FromDays(30);

        public static void AppendAccessToken(HttpResponse response, string token, bool isDevelopment)
        {
            response.Cookies.Append(AccessTokenName, token, CreateOptions(isDevelopment, delete: false));
        }

        public static void DeleteAccessToken(HttpResponse response, bool isDevelopment)
        {
            response.Cookies.Delete(AccessTokenName, CreateOptions(isDevelopment, delete: true));
        }

        private static CookieOptions CreateOptions(bool isDevelopment, bool delete)
        {
            // localhost با پورت‌های مختلف same-site محسوب می‌شود؛ Lax برای Dev کافی است.
            // در Production برای فرانت/API روی دامنه جدا به None+Secure نیاز است.
            return new CookieOptions
            {
                HttpOnly = true,
                Secure = !isDevelopment,
                SameSite = isDevelopment ? SameSiteMode.Lax : SameSiteMode.None,
                IsEssential = true,
                Path = "/",
                Expires = delete ? DateTimeOffset.UnixEpoch : DateTimeOffset.UtcNow.Add(Lifetime)
            };
        }
    }
}
