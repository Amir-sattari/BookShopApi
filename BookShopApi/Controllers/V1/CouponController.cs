using BookShopApi.Constants;
using BookShopApi.Dtos.Coupon;
using BookShopApi.Interfaces;
using BookShopApi.Mappers;
using BookShopApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShopApi.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = AppRoles.Staff)]
    public class CouponController : ControllerBase
    {
        private readonly ICouponRepository _couponRepo;

        public CouponController(ICouponRepository couponRepository)
        {
            _couponRepo = couponRepository;
        }

        [HttpGet("GetAllCoupons")]
        public async Task<ActionResult<IEnumerable<Coupon>>> GetAllAsync()
        {
            var coupons = await _couponRepo.GetAllAsync();
            return Ok(coupons.Select(c => c.ToCouponDto()).ToList());
        }

        [HttpGet("GetCouponById/{id:int}")]
        public async Task<ActionResult<Coupon>> GetByIdAsync([FromRoute] int id)
        {
            var coupon = await _couponRepo.GetByIdAsync(id);
            if (coupon == null)
                return NotFound(new { Message = "Coupon not found." });

            return Ok(coupon.ToCouponDto());
        }

        [HttpPost("CreateCoupon")]
        public async Task<ActionResult<Coupon>> CreateAsync([FromBody] CreateCouponDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var created = await _couponRepo.CreateAsync(dto);
                return Created($"api/Coupon/{created.Id}", created.ToCouponDto());
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }

        [HttpPut("UpdateCoupon/{id:int}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] UpdateCouponDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updated = await _couponRepo.UpdateAsync(id, dto);
                if (updated == null)
                    return NotFound(new { Message = "Coupon not found." });

                return NoContent();
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }

        [HttpDelete("DeleteCoupon/{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var coupon = await _couponRepo.DeactivateAsync(id);
            if (coupon == null)
                return NotFound(new { Message = "Coupon not found." });

            return NoContent();
        }
    }
}
