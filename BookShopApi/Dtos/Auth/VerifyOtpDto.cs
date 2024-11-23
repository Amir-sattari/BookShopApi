using System.ComponentModel.DataAnnotations;

namespace BookShopApi.Dtos.Auth
{
    public class VerifyOtpDto
    {
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required, MinLength(6), MaxLength(6)]
        public string OTP { get; set; } = string.Empty;
    }
}
