using System.ComponentModel.DataAnnotations;

namespace BookShopApi.Dtos.Role
{
    public class AssignRoleDto
    {
        [Required]
        public string Role { get; set; } = string.Empty;
    }
}
