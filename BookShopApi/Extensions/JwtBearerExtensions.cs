using BookShopApi.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace BookShopApi.Extensions
{
    public static class JwtBearerExtensions
    {
        public static void ReadTokenFromCookie(this JwtBearerOptions options)
        {
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    // اگر Authorization: Bearer باشد، middleware همان را می‌گیرد.
                    var header = context.Request.Headers.Authorization.FirstOrDefault();
                    if (!string.IsNullOrEmpty(header))
                        return Task.CompletedTask;

                    if (context.Request.Cookies.TryGetValue(AuthCookie.AccessTokenName, out var cookieToken)
                        && !string.IsNullOrWhiteSpace(cookieToken))
                    {
                        context.Token = cookieToken;
                    }

                    return Task.CompletedTask;
                }
            };
        }
    }
}
