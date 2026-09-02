using BookShopApi.Constants;
using BookShopApi.Dtos.BookDiscount;
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
    public class BookDiscountController : ControllerBase
    {
        private readonly IBookDiscountRepository _bookDiscountRepo;

        public BookDiscountController(IBookDiscountRepository bookDiscountRepository)
        {
            _bookDiscountRepo = bookDiscountRepository;
        }

        [HttpGet("GetByBookId/{bookId:int}")]
        public async Task<ActionResult<IEnumerable<BookDiscount>>> GetByBookIdAsync([FromRoute] int bookId)
        {
            var discounts = await _bookDiscountRepo.GetByBookIdAsync(bookId);
            return Ok(discounts.Select(d => d.ToBookDiscountDto()).ToList());
        }

        [HttpPost("Create/{bookId:int}")]
        public async Task<ActionResult<BookDiscount>> CreateAsync(
            [FromRoute] int bookId,
            [FromBody] CreateBookDiscountDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var created = await _bookDiscountRepo.CreateAsync(bookId, dto);
                return Created($"api/BookDiscount/{bookId}/{created.Id}", created.ToBookDiscountDto());
            }
            catch (ArgumentException e)
            {
                return NotFound(new { Message = e.Message });
            }
        }

        [HttpPut("Update/{bookId:int}/{id:int}")]
        public async Task<IActionResult> UpdateAsync(
            [FromRoute] int bookId,
            [FromRoute] int id,
            [FromBody] UpdateBookDiscountDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _bookDiscountRepo.UpdateAsync(bookId, id, dto);
            if (updated == null)
                return NotFound(new { Message = "Book discount not found." });

            return NoContent();
        }

        [HttpDelete("Delete/{bookId:int}/{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int bookId, [FromRoute] int id)
        {
            var discount = await _bookDiscountRepo.DeactivateAsync(bookId, id);
            if (discount == null)
                return NotFound(new { Message = "Book discount not found." });

            return NoContent();
        }
    }
}
