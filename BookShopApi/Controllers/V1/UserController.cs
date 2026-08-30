using BookShopApi.Dtos.User;
using BookShopApi.Interfaces;
using BookShopApi.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace BookShopApi.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;

        public UserController(IUserRepository userRepository)
        {
            _userRepo = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsersAsync()
        {
            var users = await _userRepo.GetAllUsersAsync();
            return Ok(users.Select(user => user.ToUserDto()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserByIdAsync([FromRoute] string id)
        {
            var user = await _userRepo.GetUserByIdAsync(id);
            if (user == null)
                return NotFound(new { ErrorMessage = $"The user with Id: {id}, Not found." });

            return Ok(user.ToUserDto());
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUserAsync([FromBody] CreateUserDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdUser = await _userRepo.CreateUserAsync(userDto);
                return Created($"api/user/{createdUser.Id}", createdUser.ToUserDto());
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(new { ErrorMessage = e.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserDto userDto, [FromRoute] string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = await _userRepo.UpdateUserAsync(userDto, id);
                if (user == null)
                    return NotFound(new { ErrorMessage = $"The user with Id: {id}, Not found." });

                return NoContent();
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(new { ErrorMessage = e.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAsync([FromRoute] string id)
        {
            var user = await _userRepo.DeleteUserAsync(id);
            if (user == null)
                return NotFound(new { ErrorMessage = $"The user with Id: {id}, Not found." });

            return NoContent();
        }
    }
}
