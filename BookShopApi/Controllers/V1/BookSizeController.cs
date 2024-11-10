using BookShopApi.Dtos.BookSize;
using BookShopApi.Interfaces;
using BookShopApi.Mappers;
using BookShopApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookShopApi.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookSizeController : ControllerBase
    {
        private readonly IBookSizeRepository _bookSizeRepo;

        public BookSizeController(IBookSizeRepository bookSizeRepository)
        {
            _bookSizeRepo = bookSizeRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookSize>>> GetAllBookSizeAsync()
        {
            var bookSizes = await _bookSizeRepo.GetAllBookSizeAsync();
            var bookSizeDto = bookSizes.Select(b => b.ToBookSizeDto());

            return Ok(bookSizeDto);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BookSize>> GetBookSizeAsync([FromRoute] int id)
        {
            var bookSize = await _bookSizeRepo.GetBookSizeByIdAsync(id);

            if (bookSize == null)
                return NotFound($"The BookSize with Id: {id}, Not found.");

            return Ok(bookSize.ToBookSizeDto());
        }

        [HttpPost]
        public async Task<ActionResult<BookSize>> CreateBookSizeAsync([FromBody] CreateBookSizeDto bookSizeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var bookSizeModel = bookSizeDto.ToBookSizeFromCreateDto();
            await _bookSizeRepo.CreateBookSizeAsync(bookSizeModel);
            return Created($"api/bookSize/{bookSizeModel.Id}", bookSizeModel.ToBookSizeDto());
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateBookSizeAsync([FromBody] UpdateBookSizeDto bookSizeDto, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var bookSize = await _bookSizeRepo.UpdateBookSizeAsync(bookSizeDto, id);

            if (bookSize == null)
                return NotFound($"The BookSize with Id: {id}, Not found.");

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteBookSizeAsync([FromRoute] int id)
        {
            var bookSize = await _bookSizeRepo.DeleteBookSizeAsync(id);

            if (bookSize == null)
                return NotFound($"The BookSize with Id: {id}, Not found.");

            return NoContent();
        }

    }
}
