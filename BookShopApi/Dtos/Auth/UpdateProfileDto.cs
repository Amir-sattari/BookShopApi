using System.ComponentModel.DataAnnotations;

namespace BookShopApi.Dtos.Auth
{
    public class UpdateProfileDto
    {
        [Required]
        [MinLength(2)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MinLength(2)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^09\d{9}$", ErrorMessage = "شماره همراه باید با ۰۹ شروع شود و ۱۱ رقم باشد.")]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
