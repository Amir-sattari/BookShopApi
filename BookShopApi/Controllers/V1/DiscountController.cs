using BookShopApi.Dtos.Discount;
using BookShopApi.Interfaces;
using BookShopApi.Mappers;
using BookShopApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookShopApi.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountRepository _discountRepo;

        public DiscountController(IDiscountRepository discountRepository)
        {
            _discountRepo = discountRepository;
        }

        [HttpGet("GetAllDiscounts")]
        public async Task<ActionResult<IEnumerable<Discount>>> GetAllDiscountsAsync()
        {
            var discounts = await _discountRepo.GetAllDiscountsAsync();
            var toDiscountDto = discounts.Select(d => d.ToDiscountDto()).ToList();
            return Ok(toDiscountDto);
        }

        [HttpGet("GetAllActiveDiscounts")]
        public async Task<ActionResult<IEnumerable<Discount>>> GetAllActiveDiscountsAsync()
        {
            var discounts = await _discountRepo.GetAllActiveDiscountsAsync();

            if (discounts == null)
                return NotFound(new { Message = "There Are No Active Discount." });
            var toDiscountDto = discounts.Select(d => d.ToDiscountDto()).ToList();
            return Ok(toDiscountDto);
        }

        [HttpGet("GetSingleActiveDiscountByName/{discountName}")]
        public async Task<ActionResult<Discount>> GetSingleActiveDiscountByNameAsync([FromRoute] string discountName)
        {
            var discount = await _discountRepo.GetSingleActiveDiscountByNameAsync(discountName);

            if (discount == null)
                return NotFound(new { Message = "There Are No Active Discount." });

            return Ok(discount.ToDiscountDto());
        }

        [HttpGet("GetDiscountById/{id:int}")]
        public async Task<ActionResult<Discount>> GetDiscountByIdAsync([FromRoute] int id)
        {
            var discount = await _discountRepo.GetDiscountByIdAsync(id);

            if (discount == null)
                return NotFound(new { Message = "Discount Not Found" });

            return Ok(discount.ToDiscountDto());
        }

        [HttpGet("GetDiscountByName/{discountName}")]
        public async Task<ActionResult<Discount>> GetDiscountByNameAsync([FromRoute] string discountName)
        {
            var discount = await _discountRepo.GetDiscountByNameAsync(discountName);

            if (discount == null)
                return NotFound(new { Message = "Discount Not Found" });

            return Ok(discount.ToDiscountDto());
        }

        [HttpPost("CreateDiscount")]
        public async Task<ActionResult<Discount>> CreateDiscountAsync([FromBody] CreateDiscountDto discountDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            
            var createdDiscount = await _discountRepo.CreateDiscountAsync(discountDto);
            return Created($"api/discount/{createdDiscount.Id}", createdDiscount.ToDiscountDto());
        }

        [HttpPut("UpdateDiscount/{id:int}")]
        public async Task<IActionResult> UpdateDiscountAsync([FromBody] UpdateDiscountDto discountDto, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedDiscount = await _discountRepo.UpdateDiscountAsync(discountDto, id);

            if (updatedDiscount == null)
                return NotFound(new { Message = "Discount Not Found" });

            return NoContent();
        }

        [HttpDelete("DeleteDiscount/{id:int}")]
        public async Task<IActionResult> DeleteDiscountAsync([FromRoute] int id)
        {
            var discount = await _discountRepo.DeleteDiscountByIdAsync(id);

            if (discount == null)
                return NotFound(new { Message = "Discount Not Found" });

            return NoContent();
        }
    }
}
