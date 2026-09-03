using BookShopApi.Helpers;
using BookShopApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShopApi.Controllers.V1
{
    [Route("api/books/{bookId:int}/notify-me")]
    [ApiController]
    [Authorize]
    public class StockNotificationController : ControllerBase
    {
        private readonly IStockNotificationService _stockNotificationService;
        private readonly IBookRepository _bookRepo;

        public StockNotificationController(
            IStockNotificationService stockNotificationService,
            IBookRepository bookRepository)
        {
            _stockNotificationService = stockNotificationService;
            _bookRepo = bookRepository;
        }

        [HttpPost]
        public async Task<IActionResult> RequestNotificationAsync([FromRoute] int bookId)
        {
            if (!this.TryResolveCurrentUser(null, out var userId, out var error))
                return error!;

            var result = await _stockNotificationService.RequestNotificationAsync(bookId, userId);

            return result.Status switch
            {
                StockNotificationStatus.Created => Ok(new { Message = result.Message }),
                StockNotificationStatus.AlreadyRequested => Conflict(new { Message = result.Message }),
                StockNotificationStatus.BookInStock => BadRequest(new { Message = result.Message }),
                StockNotificationStatus.BookNotFound => NotFound(new { Message = result.Message }),
                _ => BadRequest(new { Message = result.Message })
            };
        }

        [HttpGet("status")]
        public async Task<IActionResult> GetStatusAsync([FromRoute] int bookId)
        {
            if (!this.TryResolveCurrentUser(null, out var userId, out var error))
                return error!;

            var book = await _bookRepo.GetBookByIdAsync(bookId);
            if (book == null)
                return NotFound(new { Message = "کتاب یافت نشد." });

            var isPending = await _stockNotificationService.HasPendingRequestAsync(bookId, userId);
            return Ok(new { IsPending = isPending });
        }

        [HttpDelete]
        public async Task<IActionResult> CancelNotificationAsync([FromRoute] int bookId)
        {
            if (!this.TryResolveCurrentUser(null, out var userId, out var error))
                return error!;

            var cancelled = await _stockNotificationService.CancelPendingRequestAsync(bookId, userId);
            if (!cancelled)
                return NotFound(new { Message = "درخواست معلقی برای این کتاب یافت نشد." });

            return Ok(new { Message = "درخواست اطلاع‌رسانی لغو شد." });
        }
    }
}
