using BookShopApi.Constants;
using BookShopApi.Dtos.Role;
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
    [Authorize(Roles = AppRoles.Staff)]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IUserRepository _userRepo;

        public RoleController(IRoleService roleService, IUserRepository userRepository)
        {
            _roleService = roleService;
            _userRepo = userRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<RoleDto>> GetRoles()
        {
            return Ok(_roleService.GetRoles());
        }

        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersWithRolesAsync()
        {
            var users = (await _userRepo.GetAllUsersAsync()).ToList();
            var roleMap = await _roleService.GetUserRolesMapAsync(users.Select(user => user.Id));
            return Ok(users.Select(user => user.ToUserDto(roleMap.GetValueOrDefault(user.Id))));
        }

        [HttpPut("users/{userId}")]
        [Authorize(Roles = AppRoles.Manager)]
        public async Task<IActionResult> AssignRoleAsync([FromRoute] string userId, [FromBody] AssignRoleDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var actorUserId = User.GetUserId();
            if (string.IsNullOrWhiteSpace(actorUserId))
                return Unauthorized();

            try
            {
                await _roleService.AssignRoleAsync(userId, dto.Role, actorUserId);
                return NoContent();
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(new { ErrorMessage = e.Message });
            }
        }
    }
}
