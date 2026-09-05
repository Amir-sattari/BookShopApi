using BookShopApi.Constants;
using BookShopApi.Dtos.ShippingAddress;
using BookShopApi.Helpers;
using BookShopApi.Interfaces;
using BookShopApi.Mappers;
using BookShopApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShopApi.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShippingAddressController : ControllerBase
    {
        private readonly IShippingAddressRepository _shippingAddressRepo;

        public ShippingAddressController(IShippingAddressRepository shippingAddressRepository)
        {
            _shippingAddressRepo = shippingAddressRepository;
        }

        [HttpGet("GetAllShippingAddresses")]
        [Authorize(Roles = AppRoles.Staff)]
        public async Task<ActionResult<IEnumerable<ShippingAddress>>> GetAllShippingAddressesAsync()
        {
            var shippingAddresses = await _shippingAddressRepo.GetAllShippingAddressesAsync();
            var toAddressDto = shippingAddresses.Select(sh => sh.ToShippingAddressDto());
            return Ok(toAddressDto);
        }

        [HttpGet("Mine")]
        public async Task<ActionResult<ShippingAddress>> GetMyShippingAddressAsync()
        {
            if (!this.TryResolveCurrentUser(null, out var currentUserId, out var error))
                return error!;

            try
            {
                var shippingAddress = await _shippingAddressRepo.GetShippingAddressByUserIdAsync(currentUserId);

                if (shippingAddress == null)
                    return NotFound(new { Message = "The Shipping Address Not Found." });

                return Ok(shippingAddress.ToShippingAddressDto());
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception)
            {
                return BadRequest(new { Message = "An Unexpected Error Occurred."});
            }
        }

        [HttpPost("Mine")]
        public async Task<ActionResult<ShippingAddress>> CreateMyShippingAddressAsync([FromBody] CreateShippingAddressDto addressDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!this.TryResolveCurrentUser(null, out var currentUserId, out var error))
                return error!;

            addressDto.UserId = currentUserId;

            try
            {
                var createdShippingAddress = await _shippingAddressRepo.CreateShippingAddressAsync(addressDto);
                return Created($"api/ShippingAddress/Mine", createdShippingAddress.ToShippingAddressDto());
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception)
            {
                return BadRequest(new { Message = "An Unexpected Error Occurred." });
            }
        }

        [HttpPut("Mine")]
        public async Task<IActionResult> UpdateMyShippingAddressAsync([FromBody] UpdateShippingAddressDto addressDto)
        {
            if (!this.TryResolveCurrentUser(null, out var currentUserId, out var error))
                return error!;

            var updatedShippingAddress = await _shippingAddressRepo.UpdateShippingAddressAsync(addressDto, currentUserId);

            if (updatedShippingAddress == null)
                return NotFound(new { Message = "The Shipping Address Not Found." });

            return NoContent();
        }

        [HttpDelete("Mine")]
        public async Task<IActionResult> DeleteMyShippingAddressAsync()
        {
            if (!this.TryResolveCurrentUser(null, out var currentUserId, out var error))
                return error!;

            var updatedShippingAddress = await _shippingAddressRepo.DeleteShippingAddressByUserIdAsync(currentUserId);

            if (updatedShippingAddress == null)
                return NotFound(new { Message = "The Shipping Address Not Found." });

            return NoContent();
        }
    }
}
