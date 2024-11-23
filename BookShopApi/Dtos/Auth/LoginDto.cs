using System.ComponentModel.DataAnnotations;

namespace BookShopApi.Dtos.Auth
{
    public class LoginDto
    {
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
