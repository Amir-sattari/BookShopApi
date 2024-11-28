using BookShopApi.Dtos.Bookmark;
using BookShopApi.Interfaces;
using BookShopApi.Mappers;
using BookShopApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookShopApi.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookmarkController : ControllerBase
    {
        private readonly IBookmarkRepository _bookmarkRepo;

        public BookmarkController(IBookmarkRepository bookmarkRepository)
        {
            _bookmarkRepo = bookmarkRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddBookmarkAsync([FromBody] CreateBookmarkDto bookmarkDto)
        {
            try
            {
                await _bookmarkRepo.AddBookmarkAsync(bookmarkDto);
                return Ok(new { Message = "Bookmark Added Successfully" });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetBookmarksByUserId/{userId}")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBookmarkedBooksByBookIdAsync([FromRoute] string userId)
        {
            try
            {
                var bookmarkedBooks = await _bookmarkRepo.GetBookmarkedBooksByUserIdAsync(userId);
                if (bookmarkedBooks == null)
                    return NotFound(new { Message = "No Books Founded" });

                var bookDto = bookmarkedBooks.Select(book => book.ToBookDto($"{Request.Scheme}://{Request.Host}{book.ImageUrl}"));
                return Ok(bookDto);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetBookmarksByBookId/{bookId:int}")]
        public async Task<ActionResult<Book>> GetBookmarkedBookByBookIdAsync([FromRoute] int bookId)
        {
            try
            {
                var bookmarkedBook = await _bookmarkRepo.GetBookmarkedBookByBookIdAsync(bookId);
                if (bookmarkedBook == null)
                    return NotFound(new { Message = "The Book Not Found" });

                return Ok(bookmarkedBook.ToBookDto($"{Request.Scheme}://{Request.Host}{bookmarkedBook.ImageUrl}"));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("DeleteBookmarkByUserId/{userId}")]
        public async Task<IActionResult> DeleteBookmarkedBookByUserIdAsync([FromRoute] string userId)
        {
            try
            {
                await _bookmarkRepo.DeleteBookmarkAsync(userId);
                return Ok(new { Message = "Bookmark Removed Successfully" });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
