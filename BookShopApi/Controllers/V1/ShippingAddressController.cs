using BookShopApi.Dtos.ShippingAddress;
using BookShopApi.Interfaces;
using BookShopApi.Mappers;
using BookShopApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookShopApi.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingAddressController : ControllerBase
    {
        private readonly IShippingAddressRepository _shippingAddressRepo;

        public ShippingAddressController(IShippingAddressRepository shippingAddressRepository)
        {
            _shippingAddressRepo = shippingAddressRepository;
        }

        [HttpGet("GetAllShippingAddresses")]
        public async Task<ActionResult<IEnumerable<ShippingAddress>>> GetAllShippingAddressesAsync()
        {
            var shippingAddresses = await _shippingAddressRepo.GetAllShippingAddressesAsync();
            var toAddressDto = shippingAddresses.Select(sh => sh.ToShippingAddressDto());
            return Ok(toAddressDto);
        }

        [HttpGet("GetShippingAddressByUserId/{userId}")]
        public async Task<ActionResult<ShippingAddress>> GetShippingAddressByUserIdAsync([FromRoute] string userId)
        {
            try
            {
                var shippingAddress = await _shippingAddressRepo.GetShippingAddressByUserIdAsync(userId);

                if (shippingAddress == null)
                    return NotFound(new { Message = "The Shipping Address Not Found." });

                return Ok(shippingAddress.ToShippingAddressDto());
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception )
            {
                return BadRequest(new { Message = "An Unexpected Error Occurred."});
            }
        }

        [HttpPost("CreateShippingAddress")]
        public async Task<ActionResult<ShippingAddress>> CreateShippingAddressAsync([FromBody] CreateShippingAddressDto addressDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdShippingAddress = await _shippingAddressRepo.CreateShippingAddressAsync(addressDto);
                return Created($"api/shippingAddress/{createdShippingAddress.Id}", createdShippingAddress.ToShippingAddressDto());
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

        [HttpPut("UpdateShippingAddress/{userId}")]
        public async Task<IActionResult> UpdateShippingAddressAsync([FromBody] UpdateShippingAddressDto addressDto, [FromRoute] string userId)
        {
            var updatedShippingAddress = await _shippingAddressRepo.UpdateShippingAddressAsync(addressDto, userId);

            if (updatedShippingAddress == null)
                return NotFound(new { Message = "The Shipping Address Not Found." });

            return NoContent();
        }

        [HttpDelete("DeleteShippingAddress/{userId}")]
        public async Task<IActionResult> DeleteShippingAddressAsync([FromRoute] string userId)
        {
            var updatedShippingAddress = await _shippingAddressRepo.DeleteShippingAddressByUserIdAsync(userId);

            if (updatedShippingAddress == null)
                return NotFound(new { Message = "The Shipping Address Not Found." });

            return NoContent();
        }
    }
}
