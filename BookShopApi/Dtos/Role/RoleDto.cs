namespace BookShopApi.Dtos.Role
{
    public class RoleDto
    {
        public string Name { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public IReadOnlyList<string> Permissions { get; set; } = [];
    }
}
