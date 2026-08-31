using BookShopApi.Constants;
using BookShopApi.Data;
using BookShopApi.Dtos.Role;
using BookShopApi.Interfaces;
using BookShopApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookShopApi.Services
{
    public class RoleService : IRoleService
    {
        private static readonly IReadOnlyList<RoleDto> RoleCatalog =
        [
            new RoleDto
            {
                Name = AppRoles.Manager,
                DisplayName = "مدیر",
                Description = "دسترسی کامل به پنل مدیریت، کاربران و نقش‌ها.",
                Permissions =
                [
                    "ورود به پنل مدیریت",
                    "مدیریت کتاب‌ها و کاتالوگ",
                    "مدیریت کاربران",
                    "مدیریت نقش‌ها و دسترسی‌ها"
                ]
            },
            new RoleDto
            {
                Name = AppRoles.Admin,
                DisplayName = "ادمین",
                Description = "مدیریت فروشگاه بدون تغییر نقش کاربران.",
                Permissions =
                [
                    "ورود به پنل مدیریت",
                    "مدیریت کتاب‌ها و کاتالوگ",
                    "مدیریت کاربران"
                ]
            },
            new RoleDto
            {
                Name = AppRoles.User,
                DisplayName = "کاربر",
                Description = "خرید از فروشگاه؛ بدون دسترسی به پنل مدیریت.",
                Permissions =
                [
                    "مشاهده فروشگاه",
                    "سبد خرید و علاقه‌مندی‌های خود",
                    "آدرس ارسال و پروفایل"
                ]
            }
        ];

        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _context;

        public RoleService(UserManager<AppUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public IReadOnlyList<RoleDto> GetRoles() => RoleCatalog;

        public async Task<Dictionary<string, List<string>>> GetUserRolesMapAsync(IEnumerable<string>? userIds = null)
        {
            var userIdList = userIds?.ToList();
            var query = _context.UserRoles.AsQueryable();
            if (userIdList is { Count: > 0 })
                query = query.Where(ur => userIdList.Contains(ur.UserId));

            var userRoles = await query.ToListAsync();
            var roles = await _context.Roles.ToDictionaryAsync(role => role.Id, role => role.Name ?? string.Empty);

            return userRoles
                .GroupBy(ur => ur.UserId)
                .ToDictionary(
                    group => group.Key,
                    group => group
                        .Select(ur => roles.GetValueOrDefault(ur.RoleId) ?? string.Empty)
                        .Where(name => name.Length > 0)
                        .ToList());
        }

        public async Task<IList<string>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("کاربر پیدا نشد.");

            return await _userManager.GetRolesAsync(user);
        }

        public async Task AssignRoleAsync(string userId, string role, string actorUserId)
        {
            if (!AppRoles.IsValid(role))
                throw new InvalidOperationException("نقش انتخاب‌شده معتبر نیست.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("کاربر پیدا نشد.");

            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Contains(role) && currentRoles.Count == 1)
                return;

            if (currentRoles.Contains(AppRoles.Manager) && role != AppRoles.Manager)
            {
                var managers = await _userManager.GetUsersInRoleAsync(AppRoles.Manager);
                if (managers.Count <= 1)
                    throw new InvalidOperationException("نمی‌توان تنها مدیر سیستم را از نقش مدیر خارج کرد.");
            }

            if (currentRoles.Count > 0)
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!removeResult.Succeeded)
                    throw new InvalidOperationException("تغییر نقش انجام نشد.");
            }

            var addResult = await _userManager.AddToRoleAsync(user, role);
            if (!addResult.Succeeded)
                throw new InvalidOperationException("ثبت نقش جدید انجام نشد.");
        }
    }
}
