using BookShopApi.Constants;
using BookShopApi.Dtos.User;
using BookShopApi.Helpers;
using BookShopApi.Interfaces;
using BookShopApi.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShopApi.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly IRoleService _roleService;

        public UserController(IUserRepository userRepository, IRoleService roleService)
        {
            _userRepo = userRepository;
            _roleService = roleService;
        }

        [HttpGet]
        [Authorize(Roles = AppRoles.Staff)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsersAsync()
        {
            var users = (await _userRepo.GetAllUsersAsync()).ToList();
            var roleMap = await _roleService.GetUserRolesMapAsync(users.Select(user => user.Id));
            return Ok(users.Select(user => user.ToUserDto(roleMap.GetValueOrDefault(user.Id))));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserByIdAsync([FromRoute] string id)
        {
            var currentUserId = User.GetUserId();
            if (!User.IsStaff() && currentUserId != id)
                return Forbid();

            var user = await _userRepo.GetUserByIdAsync(id);
            if (user == null)
                return NotFound(new { ErrorMessage = $"The user with Id: {id}, Not found." });

            var roles = await _roleService.GetUserRolesAsync(user.Id);
            return Ok(user.ToUserDto(roles));
        }

        [HttpPost]
        [Authorize(Roles = AppRoles.Staff)]
        public async Task<ActionResult<UserDto>> CreateUserAsync([FromBody] CreateUserDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdUser = await _userRepo.CreateUserAsync(userDto);
                var roles = await _roleService.GetUserRolesAsync(createdUser.Id);
                return Created($"api/user/{createdUser.Id}", createdUser.ToUserDto(roles));
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(new { ErrorMessage = e.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = AppRoles.Staff)]
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
        [Authorize(Roles = AppRoles.Staff)]
        public async Task<IActionResult> DeleteUserAsync([FromRoute] string id)
        {
            var currentUserId = User.GetUserId();
            if (currentUserId == id)
                return BadRequest(new { ErrorMessage = "نمی‌توانید حساب خودتان را حذف کنید." });

            var user = await _userRepo.GetUserByIdAsync(id);
            if (user == null)
                return NotFound(new { ErrorMessage = $"The user with Id: {id}, Not found." });

            var roles = await _roleService.GetUserRolesAsync(id);
            if (roles.Contains(AppRoles.Manager))
            {
                var users = await _userRepo.GetAllUsersAsync();
                var roleMap = await _roleService.GetUserRolesMapAsync(users.Select(item => item.Id));
                var managerCount = roleMap.Values.Count(list => list.Contains(AppRoles.Manager));
                if (managerCount <= 1)
                    return BadRequest(new { ErrorMessage = "نمی‌توان تنها مدیر سیستم را حذف کرد." });
            }

            await _userRepo.DeleteUserAsync(id);
            return NoContent();
        }
    }
}
