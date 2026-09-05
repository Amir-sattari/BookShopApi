using BookShopApi.Dtos.Auth;
using System.Security.Claims;

namespace BookShopApi.Interfaces
{
    public interface IProfileService
    {
        Task<ProfileOverviewDto> GetOverviewAsync(ClaimsPrincipal principal);
    }
}
