using BookShopApi.Dtos.ShippingMethod;
using BookShopApi.Interfaces;
using BookShopApi.Mappers;
using BookShopApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookShopApi.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingMethodController : ControllerBase
    {
        private readonly IShippingMethodRepository _shippingMethodRepo;

        public ShippingMethodController(IShippingMethodRepository shippingMethodRepository)
        {
            _shippingMethodRepo = shippingMethodRepository;
        }

        [HttpGet("GetAllShippingMethods")]
        public async Task<ActionResult<IEnumerable<ShippingMethod>>> GetAllShippingMethodsAsync()
        {
            var shippingMethods = await _shippingMethodRepo.GetAllShippingMethodsAsync();
            var toShippingMethodDto = shippingMethods.Select(sh => sh.ToShippingMethodDto()).ToList();
            return Ok(toShippingMethodDto);
        }

        [HttpGet("GetShippingMethodById/{id:int}")]
        public async Task<ActionResult<ShippingMethod>> GetShippingMethodByIdAsync([FromRoute] int id)
        {
            var shippingMethod = await _shippingMethodRepo.GetShippingMethodByIdAsync(id);

            if (shippingMethod == null)
                return NotFound(new { Message = "Shipping Method Not Found." });

            return Ok(shippingMethod.ToShippingMethodDto());
        }

        [HttpPost("CreateShippingMethod")]
        public async Task<ActionResult<ShippingMethod>> CreateShippingMethodAsync([FromBody] CreateShippingMethodDto methodDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdShippingMethod = await _shippingMethodRepo.CreateShippingMethodAsync(methodDto);
            return Created($"api/shippingMethod/{createdShippingMethod.Id}", createdShippingMethod.ToShippingMethodDto());
        }

        [HttpPut("UpdateShippingMethod/{id:int}")]
        public async Task<IActionResult> UpdateShippingMethodAsync([FromBody] UpdateShippingMethodDto methodDto, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedShippingMethod = await _shippingMethodRepo.UpdateShippingMethodAsync(methodDto, id);

            if (updatedShippingMethod == null)
                return NotFound(new { Message = "Shipping Method Not Found." });

            return NoContent();
        }

        [HttpDelete("DeleteShippingMethod/{id:int}")]
        public async Task<IActionResult> DeleteShippingMethodAsync([FromRoute] int id)
        {
            var shippingMethod = await _shippingMethodRepo.DeleteShippingMethodAsync(id);

            if (shippingMethod == null)
                return NotFound(new { Message = "Shipping Method Not Found." });

            return NoContent();
        }
    }
}
