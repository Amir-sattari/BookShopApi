namespace BookShopApi.Dtos.Auth
{
    public class LoginResponseDto
    {
        public string UserId { get; set; } = string.Empty;
        public string OTP { get; set; } = string.Empty;
        public DateTime? OTPExpiry { get; set; }
    }
}
