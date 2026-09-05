using BookShopApi.Dtos.Bookmark;
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
            if (!this.TryResolveCurrentUser(null, out var currentUserId, out var error))
                return error!;

            bookmarkDto.UserId = currentUserId;

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

        [HttpGet("Mine")]
        public async Task<ActionResult<IEnumerable<Book>>> GetMyBookmarksAsync()
        {
            if (!this.TryResolveCurrentUser(null, out var currentUserId, out var error))
                return error!;

            try
            {
                var bookmarkedBooks = await _bookmarkRepo.GetBookmarkedBooksByUserIdAsync(currentUserId);
                var bookDto = bookmarkedBooks.Select(book => book.ToBookDto($"{Request.Scheme}://{Request.Host}{book.ImageUrl}"));
                return Ok(bookDto);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("Mine/books/{bookId:int}")]
        public async Task<ActionResult<Book>> GetMyBookmarkedBookAsync([FromRoute] int bookId)
        {
            if (!this.TryResolveCurrentUser(null, out var currentUserId, out var error))
                return error!;

            try
            {
                var bookmarkedBooks = await _bookmarkRepo.GetBookmarkedBooksByUserIdAsync(currentUserId);
                var bookmarkedBook = bookmarkedBooks.FirstOrDefault(book => book.Id == bookId);
                if (bookmarkedBook == null)
                    return NotFound(new { Message = "The Book Not Found" });

                return Ok(bookmarkedBook.ToBookDto($"{Request.Scheme}://{Request.Host}{bookmarkedBook.ImageUrl}"));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("Mine/{bookId:int}")]
        public async Task<IActionResult> DeleteMyBookmarkAsync([FromRoute] int bookId)
        {
            if (!this.TryResolveCurrentUser(null, out var currentUserId, out var error))
                return error!;

            try
            {
                await _bookmarkRepo.DeleteBookmarkAsync(currentUserId, bookId);
                return Ok(new { Message = "Bookmark Removed Successfully" });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
