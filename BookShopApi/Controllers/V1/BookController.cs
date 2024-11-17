using BookShopApi.Dtos.Book;
using BookShopApi.Interfaces;
using BookShopApi.Mappers;
using BookShopApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookShopApi.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepo;

        public BookController(IBookRepository bookRepository)
        {
            _bookRepo = bookRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetAllBooksAsync()
        {
            var books = await _bookRepo.GetAllBooksAsync();
            var booksDto = books.Select(b => b.ToBookDto($"{Request.Scheme}://{Request.Host}{b.ImageUrl}")).ToList();
            return Ok(booksDto);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Book>> GetBookByIdAsync([FromRoute] int id)
        {
            var book = await _bookRepo.GetBookByIdAsync(id);

            if (book == null)
                return NotFound(new { ErrorMessage = $"The book with Id: {id}, Not found." });

            return Ok(book.ToBookDto($"{Request.Scheme}://{Request.Host}{book.ImageUrl}"));
        }

        [HttpPost]
        public async Task<ActionResult<Book>> CreateBookAsync([FromForm] CreateBookDto bookDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdBook = await _bookRepo.CreateBookAsync(bookDto);
                return Created($"api/book/{createdBook.Id}", createdBook.ToBookDto($"{Request.Scheme}://{Request.Host}{createdBook.ImageUrl}"));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { ErrorMessage = e.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { ErrorMessage = "An unexpected error occurred." });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateBookAsync([FromForm] UpdateBookDto bookDto, [FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var book = await _bookRepo.UpdateBookAsync(bookDto, id);

                if (book == null)
                    return NotFound(new { ErrorMessage = $"The book with Id: {id}, Not found." });

                return NoContent();
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { ErrorMessage = e.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { ErrorMessage = "An unexpected error occurred." });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteBookAsync([FromRoute] int id)
        {
            var book = await _bookRepo.DeleteBookAsync(id);

            if (book == null)
                return NotFound(new { ErrorMessage = $"The book with Id: {id}, Not found." });

            return NoContent();
        }
    }
}