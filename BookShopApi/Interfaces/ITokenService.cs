using BookShopApi.Models;

namespace BookShopApi.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(AppUser user);
    }
}
