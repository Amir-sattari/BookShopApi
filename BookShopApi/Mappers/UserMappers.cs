using BookShopApi.Dtos.User;
using BookShopApi.Helpers;
using BookShopApi.Models;

namespace BookShopApi.Mappers
{
    public static class UserMappers
    {
        public static UserDto ToUserDto(this AppUser user)
        {
            var (firstName, lastName) = UserNameHelper.Split(user.UserName);

            return new UserDto
            {
                Id = user.Id,
                FirstName = firstName,
                LastName = lastName,
                UserName = user.UserName ?? string.Empty,
                PhoneNumber = user.PhoneNumber ?? string.Empty,
                IsVerified = user.IsVerified,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }
    }
}
