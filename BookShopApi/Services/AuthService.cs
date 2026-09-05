using BookShopApi.Constants;
using BookShopApi.Dtos.Auth;
using BookShopApi.Helpers;
using BookShopApi.Interfaces;
using BookShopApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
                UserName = UserNameHelper.Combine(dto.FirstName, dto.LastName),
                PhoneNumber = dto.PhoneNumber.Trim(),
                IsVerified = false
            };

            var createdUser = await _userManager.CreateAsync(user);
            if (!createdUser.Succeeded)
                throw new InvalidOperationException(UserNameHelper.ToIdentityError(createdUser));

            string otp = OTPHelper.GenerateOTP();
            user.OTP = otp;
            user.OPTExpiry = DateTime.UtcNow.AddMinutes(2);

            await _userManager.UpdateAsync(user);

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
            await EnsureUserRoleAsync(user);

            return await _tokenService.CreateTokenAsync(user);
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

            await EnsureUserRoleAsync(user);
            return await _tokenService.CreateTokenAsync(user);
        }

        public async Task<CurrentUserDto> GetCurrentUserAsync(ClaimsPrincipal principal)
        {
            var userId = principal.GetUserId();
            if (string.IsNullOrWhiteSpace(userId))
                throw new InvalidOperationException("User Not Found.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User Not Found.");

            var roles = await _userManager.GetRolesAsync(user);
            var (firstName, lastName) = UserNameHelper.Split(user.UserName);
            return new CurrentUserDto
            {
                Id = user.Id,
                FirstName = firstName,
                LastName = lastName,
                UserName = user.UserName ?? string.Empty,
                PhoneNumber = user.PhoneNumber ?? string.Empty,
                IsVerified = user.IsVerified,
                CreatedAt = user.CreatedAt,
                Roles = roles,
                CanAccessAdmin = roles.Any(AppRoles.IsStaff)
            };
        }

        public async Task<CurrentUserDto> UpdateCurrentUserAsync(ClaimsPrincipal principal, UpdateProfileDto dto)
        {
            var userId = principal.GetUserId();
            if (string.IsNullOrWhiteSpace(userId))
                throw new InvalidOperationException("User Not Found.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User Not Found.");

            var phone = dto.PhoneNumber.Trim();
            var phoneTaken = await _userManager.Users.AnyAsync(u => u.PhoneNumber == phone && u.Id != userId);
            if (phoneTaken)
                throw new InvalidOperationException("این شماره همراه قبلاً ثبت شده است.");

            user.UserName = UserNameHelper.Combine(dto.FirstName, dto.LastName);
            user.PhoneNumber = phone;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new InvalidOperationException(UserNameHelper.ToIdentityError(result));

            return await GetCurrentUserAsync(principal);
        }

        private async Task EnsureUserRoleAsync(AppUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Count > 0)
                return;

            var hasManager = (await _userManager.GetUsersInRoleAsync(AppRoles.Manager)).Count > 0;
            var role = hasManager ? AppRoles.User : AppRoles.Manager;
            await _userManager.AddToRoleAsync(user, role);
        }
    }
}
