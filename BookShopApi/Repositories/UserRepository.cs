using BookShopApi.Constants;
using BookShopApi.Data;
using BookShopApi.Dtos.Common;
using BookShopApi.Dtos.User;
using BookShopApi.Extensions;
using BookShopApi.Helpers;
using BookShopApi.Interfaces;
using BookShopApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookShopApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public UserRepository(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<PagedResult<AppUser>> GetUsersAsync(PagedQuery query)
        {
            query.Normalize();

            var users = _userManager.Users
                .AsNoTracking()
                .ApplySearch(query.Search)
                .ApplySort(query.SortKey, query.SortDirection, applyDefault: true);

            return await users.ToPagedResultAsync(query);
        }

        public async Task<AppUser?> GetUserByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<AppUser> CreateUserAsync(CreateUserDto userDto)
        {
            await EnsurePhoneIsUniqueAsync(userDto.PhoneNumber);

            var user = new AppUser
            {
                UserName = UserNameHelper.Combine(userDto.FirstName, userDto.LastName),
                PhoneNumber = userDto.PhoneNumber.Trim(),
                IsVerified = userDto.IsVerified
            };

            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
                throw new InvalidOperationException(UserNameHelper.ToIdentityError(result));

            await _userManager.AddToRoleAsync(user, AppRoles.User);
            return user;
        }

        public async Task<AppUser?> UpdateUserAsync(UpdateUserDto userDto, string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return null;

            await EnsurePhoneIsUniqueAsync(userDto.PhoneNumber, id);

            user.UserName = UserNameHelper.Combine(userDto.FirstName, userDto.LastName);
            user.PhoneNumber = userDto.PhoneNumber.Trim();
            user.IsVerified = userDto.IsVerified;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new InvalidOperationException(UserNameHelper.ToIdentityError(result));

            return user;
        }

        public async Task<AppUser?> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return null;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return user;
        }

        private async Task EnsurePhoneIsUniqueAsync(string phoneNumber, string? excludeUserId = null)
        {
            var exists = await _userManager.Users.AnyAsync(user =>
                user.PhoneNumber == phoneNumber && user.Id != excludeUserId);

            if (exists)
                throw new InvalidOperationException("این شماره همراه قبلاً ثبت شده است.");
        }
    }
}
