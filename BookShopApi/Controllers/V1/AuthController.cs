using BookShopApi.Dtos.Auth;
using BookShopApi.Helpers;
using BookShopApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShopApi.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IProfileService _profileService;
        private readonly IHostEnvironment _environment;

        public AuthController(
            IAuthService authService,
            IProfileService profileService,
            IHostEnvironment environment)
        {
            _authService = authService;
            _profileService = profileService;
            _environment = environment;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<RegisterResponseDto>> RegisterUserAsync([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var registeredUser = await _authService.RegisterUserAsync(registerDto);
                return Ok(registeredUser);
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }

        [HttpPost("ValidateRegister")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDto verifyOtpDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var token = await _authService.ValidateRegisterAsync(verifyOtpDto);
                AuthCookie.AppendAccessToken(Response, token, _environment.IsDevelopment());
                return Ok(new { Message = "Verified Successfully" });
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponseDto>> LoginUserAsync([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var loggedInUser = await _authService.SendLoginOtpAsync(loginDto);
                return Ok(loggedInUser);
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }

        [HttpPost("validateLogin")]
        public async Task<IActionResult> ValidateLogin([FromBody] VerifyOtpDto verifyOtpDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var token = await _authService.ValidateLoginAsync(verifyOtpDto);
                AuthCookie.AppendAccessToken(Response, token, _environment.IsDevelopment());
                return Ok(new { Message = "Login successful" });
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }

        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            AuthCookie.DeleteAccessToken(Response, _environment.IsDevelopment());
            return Ok(new { Message = "Logged out" });
        }

        [Authorize]
        [HttpGet("Me")]
        public async Task<ActionResult<CurrentUserDto>> GetCurrentUserAsync()
        {
            try
            {
                return Ok(await _authService.GetCurrentUserAsync(User));
            }
            catch (InvalidOperationException)
            {
                return Unauthorized();
            }
        }

        [Authorize]
        [HttpGet("Me/Overview")]
        public async Task<ActionResult<ProfileOverviewDto>> GetProfileOverviewAsync()
        {
            try
            {
                return Ok(await _profileService.GetOverviewAsync(User));
            }
            catch (InvalidOperationException)
            {
                return Unauthorized();
            }
        }

        [Authorize]
        [HttpPut("Me")]
        public async Task<ActionResult<CurrentUserDto>> UpdateCurrentUserAsync([FromBody] UpdateProfileDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                return Ok(await _authService.UpdateCurrentUserAsync(User, dto));
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(new { ErrorMessage = e.Message });
            }
        }
    }
}
