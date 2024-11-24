using BookShopApi.Dtos.Auth;
using BookShopApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookShopApi.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
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
                var token = await _authService.VerifyOtpAsync(verifyOtpDto);
                return Ok(new { Message = "Verified Successfully", token });
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
                return Ok(new { Message = "Login successful", token});
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }
    }
}
