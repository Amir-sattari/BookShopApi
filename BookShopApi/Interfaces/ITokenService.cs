using BookShopApi.Models;

namespace BookShopApi.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
