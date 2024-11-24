using BookShopApi.Dtos.Auth;
using BookShopApi.Helpers;
using BookShopApi.Interfaces;
using BookShopApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookShopApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public AuthService(UserManager<AppUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<RegisterResponseDto> RegisterUserAsync(RegisterDto dto)
        {
            var existingPhoneNumber = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == dto.PhoneNumber);
            if (existingPhoneNumber != null)
                throw new InvalidOperationException($"User Registration Failed: phone number is exist");

            var user = new AppUser
            {
                UserName = dto.Username,
                PhoneNumber = dto.PhoneNumber,
                IsVerified = false
            };

            var createdUser = await _userManager.CreateAsync(user);
            if (!createdUser.Succeeded)
            {
                var errors = string.Join(", ", createdUser.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"User Registration Failed: {errors}");
            }

            string otp = OTPHelper.GenerateOTP();
            user.OTP = otp;
            user.OPTExpiry = DateTime.UtcNow.AddMinutes(2);

            var updatedUser = await _userManager.UpdateAsync(user);

            return new RegisterResponseDto
            {
                UserId = user.Id,
                OTP = user.OTP,
                OTPExpiry = user.OPTExpiry
            };
        }

        public async Task<string> ValidateRegisterAsync(VerifyOtpDto dto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == dto.PhoneNumber);
            if (user == null)
                throw new InvalidOperationException("User Not Found");

            if (user.OPTExpiry < DateTime.UtcNow)
                throw new InvalidOperationException("OTP Expired");

            if (!OTPHelper.VerifyOTP(user.OTP, dto.OTP))
                throw new InvalidOperationException("Invalid OTP");

            user.IsVerified = true;
            user.OTP = string.Empty;
            user.OPTExpiry = null;

            await _userManager.UpdateAsync(user);

            return _tokenService.CreateToken(user);
        }

        public async Task<LoginResponseDto> SendLoginOtpAsync(LoginDto dto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == dto.PhoneNumber);
            if (user == null || !user.IsVerified)
                throw new InvalidOperationException("User Not Found or Not Verified.");

            var otp = OTPHelper.GenerateOTP();
            user.OTP = otp;
            user.OPTExpiry = DateTime.UtcNow.AddMinutes(2);

            await _userManager.UpdateAsync(user);

            return new LoginResponseDto
            {
                UserId = user.Id,
                OTP = user.OTP,
                OTPExpiry = user.OPTExpiry
            };
        }

        public async Task<string> ValidateLoginAsync(VerifyOtpDto dto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == dto.PhoneNumber);
            if (user == null)
                throw new InvalidOperationException("User Not Found.");

            if (user.OPTExpiry < DateTime.UtcNow)
                throw new InvalidOperationException("OTP Expired");

            if (!OTPHelper.VerifyOTP(user.OTP, dto.OTP))
                throw new InvalidOperationException("Invalid OTP");

            return _tokenService.CreateToken(user);
        
        }
    }
}
