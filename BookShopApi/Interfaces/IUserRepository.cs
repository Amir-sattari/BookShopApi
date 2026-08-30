using BookShopApi.Dtos.User;
using BookShopApi.Models;

namespace BookShopApi.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<AppUser>> GetAllUsersAsync();
        Task<AppUser?> GetUserByIdAsync(string id);
        Task<AppUser> CreateUserAsync(CreateUserDto userDto);
        Task<AppUser?> UpdateUserAsync(UpdateUserDto userDto, string id);
        Task<AppUser?> DeleteUserAsync(string id);
    }
}
