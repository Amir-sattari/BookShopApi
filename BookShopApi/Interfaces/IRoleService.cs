using BookShopApi.Dtos.Role;

namespace BookShopApi.Interfaces
{
    public interface IRoleService
    {
        IReadOnlyList<RoleDto> GetRoles();
        Task<Dictionary<string, List<string>>> GetUserRolesMapAsync(IEnumerable<string>? userIds = null);
        Task<IList<string>> GetUserRolesAsync(string userId);
        Task AssignRoleAsync(string userId, string role, string actorUserId);
    }
}
