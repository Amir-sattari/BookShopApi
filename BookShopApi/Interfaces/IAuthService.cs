using BookShopApi.Dtos.Auth;
using BookShopApi.Models;

namespace BookShopApi.Interfaces
{
    public interface IAuthService
    {
        Task<RegisterResponseDto> RegisterUserAsync(RegisterDto dto);
        Task<string> ValidateRegisterAsync(VerifyOtpDto dto);
        Task<LoginResponseDto> SendLoginOtpAsync(LoginDto dto);
        Task<string> ValidateLoginAsync(VerifyOtpDto dto);
    }
}
